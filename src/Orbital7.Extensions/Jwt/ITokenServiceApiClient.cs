using Orbital7.Extensions.Apis;

namespace Orbital7.Extensions.Jwt;

public interface ITokenServiceApiClient<TTokenInfo> :
    IApiClient
    where TTokenInfo : TokenInfo
{
    Task<TTokenInfo> GetTokenAsync(
        GetTokenInput input,
        CancellationToken cancellationToken = default);

    Task<RevokedTokenInfo?> RevokeTokenAsync(
        RevokeTokenInput input,
        CancellationToken cancellationToken = default);

    Task<string> EnsureValidAccessTokenAsync(
        CancellationToken cancellationToken = default,
        DateTime? nowUtc = null);
}
