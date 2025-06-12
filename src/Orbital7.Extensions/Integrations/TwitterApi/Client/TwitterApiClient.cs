namespace Orbital7.Extensions.Integrations.TwitterApi;

// Authorization: https://developer.twitter.com/en/docs/authentication/oauth-2-0/user-access-token
public class TwitterApiClient :
    OAuthApiClientBase, ITwitterApiClient
{
    protected override string OAuthTokenEndpointUrl => "https://api.twitter.com/2/oauth2/token";

    public TwitterApiClient(
        IServiceProvider serviceProvider,
        IHttpClientFactory httpClientFactory,
        string clientId,
        TokenInfo tokenInfo) :
        base(serviceProvider, httpClientFactory, clientId, tokenInfo)
    {
        
    }

    public string GetAuthorizationUrl(
        string redirectUri,
        string scope,
        string state,
        string codeChallenge)
    {
        return $"https://twitter.com/i/oauth2/authorize?" +
            $"response_type=code&" +
            $"client_id={this.ClientId}&" +
            $"redirect_uri={redirectUri}&" +
            $"scope={scope.UrlEncode()}&" +
            $"state={state}&" +
            $"code_challenge={codeChallenge}&" +
            $"code_challenge_method=plain";
    }

    public async Task<TokenInfo> ObtainTokenAsync(
        string authorizationCode,
        string redirectUri,
        string codeVerifier,
        CancellationToken cancellationToken = default)
    {
        var request = new List<KeyValuePair<string, string>>()
        {
            new KeyValuePair<string, string>("code", authorizationCode),
            new KeyValuePair<string, string>("grant_type", "authorization_code"),
            new KeyValuePair<string, string>("client_id", this.ClientId),
            new KeyValuePair<string, string>("redirect_uri", redirectUri),
            new KeyValuePair<string, string>("code_verifier", codeVerifier),
        };

        return await SendObtainTokenRequestAsync(request, cancellationToken);
    }

    protected override List<KeyValuePair<string, string>> GetRefreshTokenRequest()
    {
        if (!this.TokenInfo.RefreshToken.HasText())
        {
            throw new Exception("Refresh token is not set.");
        }

        return new List<KeyValuePair<string, string>>()
        {
            new KeyValuePair<string, string>("grant_type", "refresh_token"),
            new KeyValuePair<string, string>("client_id", this.ClientId),
            new KeyValuePair<string, string>("refresh_token", this.TokenInfo.RefreshToken),
        };
    }

    protected override Exception CreateUnsuccessfulResponseException(
        HttpResponseMessage httpResponse,
        string responseBody)
    {
        var errorResponse = JsonSerializationHelper.DeserializeFromJson<ErrorResponse>(responseBody);
        return new Exception(errorResponse?.ToString());
    }
}
