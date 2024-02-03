namespace Orbital7.Extensions.Integrations;

public interface IOAuthApiClient :
    IApiClient
{
    event OAuthTokenInfoUpdatedHandler TokenInfoUpdated;

    Task<OAuthTokenInfo> RefreshAccessTokenAsync();
}
