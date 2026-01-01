using System.Security.Claims;

namespace Orbital7.Extensions.Jwt;

public interface ITokenService<TTokenInfo, TGetTokenInput>
    where TTokenInfo : TokenInfo
{
    Task<TTokenInfo> GetTokenAsync(
        TGetTokenInput input);

    Task<TTokenInfo> RefreshTokenAsync(
        RefreshTokenInput input);

    Task<RevokedTokenInfo?> RevokeTokenAsync(
        RevokeTokenInput input);

    Task<List<RevokedTokenInfo>> RevokeExpiredTokensAsync(
        DateTime? nowUtc = null);

    Task<List<RevokedTokenInfo>> RevokeTokensLastRefreshedBeforeAsync(
        DateTime minimumLastRefreshedDateTimeUtc);

    Task<bool> IsTokenGrantValidAsync(
        ClaimsPrincipal principal);

    ClaimsPrincipal GetPrincipalFromAccessToken(
        string accessToken);
}
