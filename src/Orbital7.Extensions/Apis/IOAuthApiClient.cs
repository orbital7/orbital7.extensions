namespace Orbital7.Extensions.Apis;

public interface IOAuthApiClient :
    IApiClient
{
    string GetAuthorizationUrl(
        string? state = null);

    Task<string> EnsureValidAccessTokenAsync(
        CancellationToken cancellationToken = default,
        DateTime? nowUtc = null);
}
