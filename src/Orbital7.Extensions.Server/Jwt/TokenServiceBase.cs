using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace Orbital7.Extensions.Jwt;

public abstract class TokenServiceBase<TDbContext, TKey, TUser, TTokenGrant> :
    ITokenService
    where TDbContext : DbContext
    where TKey : IEquatable<TKey> 
    where TUser : IdentityUser<TKey>, IEntity<TKey>
    where TTokenGrant : class, ITokenGrantEntity<TKey, TUser>, new()
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

    protected abstract Task<bool> IsUserActiveAsync(
        TUser user);

    public async Task<TokenInfo> ObtainTokenAsync(
        ObtainTokenInput input)
    {
        input.AssertIsComplete();

        var user = await this.UserManager.FindByNameAsync(input.Username);
        if (user != null && await IsUserActiveAsync(user))
        {
            // Verify password.
            var passwordIsValid = await this.UserManager.CheckPasswordAsync(
                user,
                input.Password);
            if (passwordIsValid)
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
                var tokenInfo = CreateTokenInfo(
                    user, 
                    tokenGrant, 
                    tokenGrantConfig);

                // Save.
                await this.Context.SaveChangesAsync();

                return tokenInfo;
            }
        }

        throw new SecurityTokenException(this.InvalidTokenCredentialsMessage);
    }

    public async Task<TokenInfo> RefreshTokenAsync(
        RefreshTokenInput input)
    {
        input.AssertIsComplete();

        var tokenGrantConfig = GetTokenGrantConfig();

        var principal = GetPrincipalFromExpiredToken(
            input.AccessToken,
            tokenGrantConfig);

        if (principal != null)
        {
            var user = await this.UserManager.GetUserAsync(principal);
            if (user != null && await IsUserActiveAsync(user))
            {
                var tokenGrant = await this.TokenGrants
                    .Where(x =>
                        x.Id.Equals(user.Id) &&
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
                    var tokenInfo = CreateTokenInfo(
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

    public async Task<bool> IsTokenGrantValidAsync(
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

    public async Task<bool> RevokeTokenAsync(
        string refreshToken)
    {
        var tokenGrant = await this.TokenGrants
            .Where(x => x.RefreshToken == refreshToken)
            .GetAsync();

        if (tokenGrant != null)
        {
            this.TokenGrants.DeleteEntity(tokenGrant);
            await this.Context.SaveChangesAsync();
            return true;
        }

        return false;
    }

    private TokenInfo CreateTokenInfo(
        TUser user,
        TTokenGrant tokenGrant,
        TokenGrantConfig tokenGrantConfig)
    {
        var authClaims = CreateClaims(user, tokenGrant);
        var accessTokenExpirationDateTimeUtc = GetExpirationDateTimeUtc(
            tokenGrantConfig.AccessTokenExpiresAfter);

        var token = CreateToken(
            authClaims,
            accessTokenExpirationDateTimeUtc,
            tokenGrantConfig);

        var accessToken = new JwtSecurityTokenHandler().WriteToken(token);

        return new TokenInfo()
        {
            AccessToken = accessToken,
            AccessTokenExpirationDateTimeUtc = accessTokenExpirationDateTimeUtc,
            RefreshToken = tokenGrant.RefreshToken,
            RefreshTokenExpirationDateTimeUtc = tokenGrant.ExpirationDateTimeUtc,
        };
    }

    private DateTime? GetExpirationDateTimeUtc(
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

    private List<Claim> CreateClaims(
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

    // Source: https://www.c-sharpcorner.com/article/jwt-authentication-with-refresh-tokens-in-net-6-0/
    private JwtSecurityToken CreateToken(
        List<Claim> authClaims,
        DateTime? accessTokenExpirationDateTimeUtc,
        TokenGrantConfig tokenGrantConfig)
    {
        tokenGrantConfig.AssertIsComplete();

        var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenGrantConfig.SigningKey));

        var token = new JwtSecurityToken(
            issuer: tokenGrantConfig.ValidIssuer,
            audience: tokenGrantConfig.ValidAudience,
            expires: accessTokenExpirationDateTimeUtc,
            claims: authClaims,
            signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
            );

        return token;
    }

    private string GenerateRefreshToken()
    {
        var randomNumber = new byte[64];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }

    private ClaimsPrincipal GetPrincipalFromExpiredToken(
        string token,
        TokenGrantConfig tokenGrantConfig)
    {
        tokenGrantConfig.AssertIsComplete();

        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateAudience = false,
            ValidateIssuer = false,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(tokenGrantConfig.SigningKey)),
            ValidateLifetime = false
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var principal = tokenHandler.ValidateToken(
            token, 
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
}
