namespace Orbital7.Extensions.Apis.XApi;

// Authorization: https://docs.x.com/fundamentals/authentication/oauth-2-0/user-access-token
public class XApiClient :
    OAuthApiClientBase<TokenInfo>, IXApiClient
{
    private readonly XApiConfig _config;
    private readonly Func<IServiceProvider, XApiConfig, Task<string>> _getAuthorizationCodeAsync;

    protected override string OAuthTokenEndpointUrl => "https://api.x.com/2/oauth2/token";

    public XApiClient(
        IServiceProvider serviceProvider,
        IHttpClientFactory httpClientFactory,
        XApiConfig config,
        TokenInfo tokenInfo,
        Func<IServiceProvider, XApiConfig, Task<string>> getAuthorizationCodeAsync,
        string? httpClientName = null) :
        base(
            serviceProvider, 
            httpClientFactory, 
            tokenInfo, 
            httpClientName: httpClientName)
    {
        _config = config;
        _getAuthorizationCodeAsync = getAuthorizationCodeAsync;
    }

    public override string GetAuthorizationUrl(
        string? state = null)
    {
        return $"https://x.com/i/oauth2/authorize?" +
            $"response_type=code&" +
            $"client_id={_config.ClientId}&" +
            $"redirect_uri={_config.RedirectUri}&" +
            $"scope={_config.Scope.UrlEncode()}&" +
            $"state={state}&" +
            $"code_challenge={_config.CodeVerifier}&" +
            $"code_challenge_method=plain";
    }

    protected override async Task<List<KeyValuePair<string, string>>> CreateGetTokenRequestAsync()
    {
        var authorizationCode = await _getAuthorizationCodeAsync(
            this.ServiceProvider,
            _config);
        ArgumentNullException.ThrowIfNull(authorizationCode, nameof(authorizationCode));

        return [
            new KeyValuePair<string, string>("code", authorizationCode),
            new KeyValuePair<string, string>("grant_type", "authorization_code"),
            new KeyValuePair<string, string>("client_id", _config.ClientId),
            new KeyValuePair<string, string>("redirect_uri", _config.RedirectUri),
            new KeyValuePair<string, string>("code_verifier", _config.CodeVerifier)
        ];
    }

    protected override List<KeyValuePair<string, string>> CreateRefreshTokenRequest(
        string refreshToken)
    {
        return [
            new KeyValuePair<string, string>("grant_type", "refresh_token"),
            new KeyValuePair<string, string>("client_id", _config.ClientId),
            new KeyValuePair<string, string>("refresh_token", refreshToken),
        ];
    }

    protected override Exception CreateUnsuccessfulResponseException(
        HttpResponseMessage httpResponse,
        string responseBody)
    {
        var errorResponse = JsonSerializationHelper.DeserializeFromJson<ErrorResponse>(responseBody);
        return new Exception(errorResponse?.ToString());
    }
}
