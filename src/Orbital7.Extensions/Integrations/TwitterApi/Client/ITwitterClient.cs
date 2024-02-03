namespace Orbital7.Extensions.Integrations.TwitterApi;

public interface ITwitterClient :
    IOAuthApiClient
{
    string GetAuthorizationUrl(
        string redirectUri,
        string scope,
        string state,
        string codeChallenge);

    Task<OAuthTokenInfo> ObtainRefreshTokenAsync(
        string authorizationCode,
        string redirectUri,
        string codeVerifier);
}
