using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace Orbital7.Extensions.Jwt;

public abstract class TokenServiceBase<TDbContext, TKey, TUser, TTokenGrant, TTokenInfo, TGetTokenInput> :
    ITokenService<TTokenInfo, TGetTokenInput>
    where TDbContext : DbContext
    where TKey : IEquatable<TKey> 
    where TUser : IdentityUser<TKey>, IEntity<TKey>
    where TTokenGrant : class, ITokenGrantEntity<TKey, TUser>, new()
    where TTokenInfo : TokenInfo, new()
    where TGetTokenInput : GetTokenInput
{
    protected TDbContext Context { get; init; }

    protected DbSet<TTokenGrant> TokenGrants { get; init; }

    protected UserManager<TUser> UserManager { get; init; }

    protected virtual string InvalidTokenCredentialsMessage => "Invalid token credentials";

    protected virtual string InvalidTokenMessage => "Invalid token";

    protected TokenServiceBase(
        TDbContext context,
        UserManager<TUser> userManager)
    {
        this.Context = context;
        this.UserManager = userManager;
        this.TokenGrants = context.Set<TTokenGrant>();
    }

    protected abstract TokenGrantConfig GetTokenGrantConfig();

    protected abstract Task<bool> IsUserAuthorizedAsync(
        TUser user,
        DateTime nowUtc);

    public virtual async Task<TTokenInfo> GetTokenAsync(
        TGetTokenInput input)
    {
        input.AssertIsComplete();

        // Find the user by username.
        var user = await this.UserManager.FindByNameAsync(input.Username);

        // Ensure we have a user, authenticate the user, and validate if
        // the user is authorized.
        if (user != null &&
            await IsUserAuthenticatedAsync(user, input) &&
            await IsUserAuthorizedAsync(user, DateTime.UtcNow))
        {
            var tokenGrantConfig = GetTokenGrantConfig();

            // Create the grant.
            var tokenGrant = new TTokenGrant()
            {
                UserId = user.Id,
                Description = input.Description,
                RefreshToken = GenerateRefreshToken(),
                ExpirationDateTimeUtc = GetExpirationDateTimeUtc(
                    tokenGrantConfig.ExpiresAfter),
                LastRefreshedDateTimeUtc = DateTime.UtcNow,
            };
            this.TokenGrants.AddEntity(tokenGrant);

            // Create the token.
            var tokenInfo = AssembleTokenInfo(
                user,
                tokenGrant,
                tokenGrantConfig);

            // Save.
            await this.Context.SaveChangesAsync();

            return tokenInfo;
        }

        throw new SecurityTokenException(this.InvalidTokenCredentialsMessage);
    }

    public virtual async Task<TTokenInfo> RefreshTokenAsync(
        RefreshTokenInput input)
    {
        input.AssertIsComplete();

        var tokenGrantConfig = GetTokenGrantConfig();

        var principal = GetPrincipalFromAccessToken(
            input.AccessToken);

        if (principal != null)
        {
            var user = await this.UserManager.GetUserAsync(principal);
            if (user != null &&
                await IsUserAuthorizedAsync(user, DateTime.UtcNow))
            {
                var tokenGrant = await this.TokenGrants
                    .Where(x =>
                        x.UserId.Equals(user.Id) &&
                        x.RefreshToken == input.RefreshToken)
                    .GetAsync();

                if (tokenGrant != null)
                {
                    // Check for expired refresh token.
                    if (tokenGrant.ExpirationDateTimeUtc.HasValue &&
                        tokenGrant.ExpirationDateTimeUtc.Value < DateTime.UtcNow)
                    {
                        throw new RefreshTokenExpiredException();
                    }

                    // Create the token.
                    var tokenInfo = AssembleTokenInfo(
                        user, 
                        tokenGrant,
                        tokenGrantConfig);

                    // Update token grant.
                    tokenGrant.ExpirationDateTimeUtc = GetExpirationDateTimeUtc(tokenGrantConfig.ExpiresAfter);
                    tokenGrant.LastRefreshedDateTimeUtc = DateTime.UtcNow;
                    this.TokenGrants.UpdateEntityProperties(
                        tokenGrant,
                        x => x.LastRefreshedDateTimeUtc,
                        x => x.ExpirationDateTimeUtc);
                    await this.Context.SaveChangesAsync();

                    return tokenInfo;
                }
            }
        }

        throw new SecurityTokenException(this.InvalidTokenMessage);
    }

    public virtual async Task<RevokedTokenInfo?> RevokeTokenAsync(
        RevokeTokenInput input)
    {
        var tokenGrant = await this.TokenGrants
            .Where(x => x.RefreshToken == input.RefreshToken)
            .GetAsync();

        if (tokenGrant != null)
        {
            this.TokenGrants.DeleteEntity(tokenGrant);
            await this.Context.SaveChangesAsync();

            return new RevokedTokenInfo
            {
                UserId = tokenGrant.UserId.ToString(),
                Description = tokenGrant.Description,
            };
        }

        return null;
    }

    public virtual async Task<List<RevokedTokenInfo>> RevokeExpiredTokensAsync(
        DateTime? nowUtc = null)
    {
        var tokenGrants = await this.TokenGrants
            .Where(x => x.ExpirationDateTimeUtc.HasValue &&
                        x.ExpirationDateTimeUtc <= (nowUtc ?? DateTime.UtcNow))
            .ToListAsync();

        return await ExecuteRevokeTokensAsync(tokenGrants);
    }

    public virtual async Task<List<RevokedTokenInfo>> RevokeTokensLastRefreshedBeforeAsync(
        DateTime minimumLastRefreshedDateTimeUtc)
    {
        var tokenGrants = await this.TokenGrants
            .Where(x => x.LastRefreshedDateTimeUtc < minimumLastRefreshedDateTimeUtc)
            .ToListAsync();

        return await ExecuteRevokeTokensAsync(tokenGrants);
    }

    public virtual async Task<bool> IsTokenGrantValidAsync(
        ClaimsPrincipal principal)
    {
        if (principal == null)
        {
            throw new ArgumentNullException(nameof(principal));
        }

        var tokenGrantId = StringHelper.ParseToType<TKey>(
            principal.FindFirstValue(JwtRegisteredClaimNames.Jti));

        if (tokenGrantId == null)
        {
            throw new Exception("No Token Grant ID found");
        }

        var tokenGrant = await this.TokenGrants.GetAsync(
            tokenGrantId,
            x => new
            {
                x.ExpirationDateTimeUtc,
            });

        return tokenGrant != null &&
            (!tokenGrant.ExpirationDateTimeUtc.HasValue ||
             tokenGrant.ExpirationDateTimeUtc > DateTime.UtcNow);
    }

    public virtual ClaimsPrincipal GetPrincipalFromAccessToken(
        string accessToken)
    {
        var tokenGrantConfig = GetTokenGrantConfig();
        tokenGrantConfig.AssertIsComplete();

        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateAudience = false,
            ValidateIssuer = false,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = TokenServiceHelper.GetSigningKey(tokenGrantConfig.SigningKey),
            ValidateLifetime = false
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var principal = tokenHandler.ValidateToken(
            accessToken,
            tokenValidationParameters,
            out SecurityToken securityToken);

        if (securityToken is not JwtSecurityToken jwtSecurityToken ||
            !jwtSecurityToken.Header.Alg.Equals(
                SecurityAlgorithms.HmacSha256,
                StringComparison.InvariantCultureIgnoreCase))
        {
            throw new SecurityTokenException(this.InvalidTokenMessage);
        }

        return principal;
    }

    protected virtual TTokenInfo AssembleTokenInfo(
        TUser user,
        TTokenGrant tokenGrant,
        TokenGrantConfig tokenGrantConfig)
    {
        var authClaims = GetTokenClaims(user, tokenGrant);
        var accessTokenExpirationDateTimeUtc = GetExpirationDateTimeUtc(
            tokenGrantConfig.AccessTokenExpiresAfter);

        var jwtAccessToken = CreateJwtSecurityToken(
            authClaims,
            accessTokenExpirationDateTimeUtc,
            tokenGrantConfig);

        var accessToken = new JwtSecurityTokenHandler().WriteToken(jwtAccessToken);

        return new TTokenInfo()
        {
            AccessToken = accessToken,
            AccessTokenExpirationDateTimeUtc = accessTokenExpirationDateTimeUtc,
            RefreshToken = tokenGrant.RefreshToken,
            RefreshTokenExpirationDateTimeUtc = tokenGrant.ExpirationDateTimeUtc,
        };
    }

    protected virtual async Task<bool> IsUserAuthenticatedAsync(
        TUser user,
        TGetTokenInput input)
    {
        input.AssertIsComplete();

        return await this.UserManager.CheckPasswordAsync(
            user, 
            input.Password);
    }

    protected DateTime? GetExpirationDateTimeUtc(
        TimeSpan? expiresAfter)
    {
        if (expiresAfter.HasValue)
        {
            return DateTime.UtcNow.Add(expiresAfter.Value);
        }
        else
        {
            return null;
        }
    }

    protected virtual List<Claim> GetTokenClaims(
        TUser user,
        TTokenGrant tokenGrant)
    {
        var userId = user.Id.ToString();
        var tokenGrantId = tokenGrant.Id.ToString();

        if (userId.HasText() &&
            user.UserName.HasText() &&
            user.Email.HasText() &&
            tokenGrantId.HasText())
        {
            return new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, userId),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, tokenGrantId),
            };
        }
        else
        {
            throw new ArgumentException("Provided User or Token Grant is incomplete");
        }
    }

    protected virtual JwtSecurityToken CreateJwtSecurityToken(
        List<Claim> authClaims,
        DateTime? accessTokenExpirationDateTimeUtc,
        TokenGrantConfig tokenGrantConfig)
    {
        tokenGrantConfig.AssertIsComplete();

        var token = new JwtSecurityToken(
            issuer: tokenGrantConfig.ValidIssuer,
            audience: tokenGrantConfig.ValidAudience,
            expires: accessTokenExpirationDateTimeUtc,
            claims: authClaims,
            signingCredentials: new SigningCredentials(
                TokenServiceHelper.GetSigningKey(tokenGrantConfig.SigningKey), 
                SecurityAlgorithms.HmacSha256));

        return token;
    }

    protected virtual string GenerateRefreshToken()
    {
        var randomNumber = new byte[64];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }

    protected virtual async Task<List<RevokedTokenInfo>> ExecuteRevokeTokensAsync(
        List<TTokenGrant> tokenGrants)
    {
        foreach (var tokenGrant in tokenGrants)
        {
            this.TokenGrants.DeleteEntity(tokenGrant);
        }

        await this.Context.SaveChangesAsync();

        return tokenGrants
            .Select(x => new RevokedTokenInfo
            {
                UserId = x.UserId.ToString(),
                Description = x.Description,
            })
            .ToList();
    }
}
