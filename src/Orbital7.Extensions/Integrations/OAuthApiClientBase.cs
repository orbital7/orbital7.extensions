namespace Orbital7.Extensions.Integrations;

public abstract class OAuthApiClientBase :
    ApiClient
{
    private IServiceProvider ServiceProvider { get; set; }

    protected string ClientId { get; private set; }

    public TokenInfo TokenInfo { get; private set; }

    protected abstract string OAuthTokenEndpointUrl { get; }

    protected virtual int OAuthAccessTokenPreExpirationCutoffInMinutes => 10;

    private Func<IServiceProvider, TokenInfo, Task> OnTokenInfoUpdated { get; set; }

    protected OAuthApiClientBase(
        IServiceProvider serviceProvider,
        IHttpClientFactory httpClientFactory,
        string clientId,
        TokenInfo tokenInfo,
        Func<IServiceProvider, TokenInfo, Task> onTokenInfoUpdated = null) :
        base(httpClientFactory)
    {
        this.ServiceProvider = serviceProvider;
        this.ClientId = clientId;
        this.TokenInfo = tokenInfo;
        this.OnTokenInfoUpdated = onTokenInfoUpdated;
    }

    protected async Task<TokenInfo> SendObtainTokenRequestAsync(
        List<KeyValuePair<string, string>> request)
    {
        var response = await SendPostRequestUrlEncodedAsync<OAuthTokenResponse>(
            this.OAuthTokenEndpointUrl,
            request);

        await UpdateTokenInfoAsync(response);

        return this.TokenInfo;
    }

    private async Task<TokenInfo> RefreshTokenAsync()
    {
        var request = GetRefreshTokenRequest();

        var response = await SendPostRequestUrlEncodedAsync<OAuthTokenResponse>(
            this.OAuthTokenEndpointUrl,
            request);

        await UpdateTokenInfoAsync(response);

        return this.TokenInfo;
    }

    protected abstract List<KeyValuePair<string, string>> GetRefreshTokenRequest();

    private async Task UpdateTokenInfoAsync(
        OAuthTokenResponse response)
    {
        if (response.RefreshToken.HasText())
        {
            this.TokenInfo.RefreshToken = response.RefreshToken;
        }

        this.TokenInfo.AccessToken = response.AccessToken;
        this.TokenInfo.AccessTokenExpirationDateTimeUtc = DateTime.UtcNow.AddSeconds(response.ExpiresIn);

        if (this.OnTokenInfoUpdated != null)
        {
            await this.OnTokenInfoUpdated.Invoke(this.ServiceProvider, this.TokenInfo);
        }
    }

    protected async Task<TokenInfo> EnsureValidAccessTokenAsync(
        DateTime? nowUtc = null)
    {
        // Ensure we have a specified refresh token.
        if (!this.TokenInfo.RefreshToken.HasText())
            throw new Exception($"Token info does not contain a specified refresh token");

        // Calculate an expiration cutoff as the next X minutes.
        var cutoffUtc = (nowUtc ?? DateTime.UtcNow)
            .AddMinutes(this.OAuthAccessTokenPreExpirationCutoffInMinutes);

        // Check for either a missing or expiring access token.
        if (!this.TokenInfo.AccessToken.HasText() ||
            !this.TokenInfo.AccessTokenExpirationDateTimeUtc.HasValue ||
            this.TokenInfo.AccessTokenExpirationDateTimeUtc.Value < cutoffUtc)
        {
            await RefreshTokenAsync();
        }

        return this.TokenInfo;
    }

    protected override async Task BeforeCreateRequestAsync(
        Uri uri)
    {
        if (IsAuthorizationRequired(uri))
        {
            await EnsureValidAccessTokenAsync();
        }
    }

    protected override void AddRequestHeaders(
        HttpRequestMessage httpRequest)
    {
        if (IsAuthorizationRequired(httpRequest.RequestUri))
        {
            httpRequest.AddBearerTokenAuthorizationHeader(
                this.TokenInfo.AccessToken);
        }
    }

    protected virtual bool IsAuthorizationRequired(
        Uri uri)
    {
        if (uri.ToString().Equals(this.OAuthTokenEndpointUrl, StringComparison.CurrentCultureIgnoreCase))
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    private class OAuthTokenResponse
    {
        [JsonPropertyName("expires_in")]
        public int ExpiresIn { get; set; }

        [JsonPropertyName("access_token")]
        public string AccessToken { get; set; }

        [JsonPropertyName("refresh_token")]
        public string RefreshToken { get; set; }

        [JsonPropertyName("token_type")]
        public string TokenType { get; set; }

        [JsonPropertyName("scope")]
        public string Scope { get; set; }
    }
}
