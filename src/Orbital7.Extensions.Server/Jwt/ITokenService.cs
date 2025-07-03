using System.Security.Claims;

namespace Orbital7.Extensions.Jwt;

public interface ITokenService
{
    Task<TokenInfo> ObtainTokenAsync(
        ObtainTokenInput input);

    Task<TokenInfo> RefreshTokenAsync(
        RefreshTokenInput input);

    Task<bool> RevokeTokenAsync(
        string refreshToken);

    Task<bool> IsTokenGrantValidAsync(
        ClaimsPrincipal principal);
}
