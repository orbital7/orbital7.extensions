namespace Orbital7.Extensions.Integrations;

public delegate void OAuthTokenInfoUpdatedHandler(
    OAuthTokenInfo tokenInfo);

public abstract class OAuthApiClientBase :
    ApiClient
{
    protected string ClientId { get; private set; }

    public OAuthTokenInfo TokenInfo { get; private set; }

    protected abstract string OAuthTokenEndpointUrl { get; }

    protected virtual int OAuthTokenPreExpirationCutoffInMinutes => 10;

    public event OAuthTokenInfoUpdatedHandler TokenInfoUpdated;

    protected OAuthApiClientBase(
        string clientId,
        OAuthTokenInfo tokenInfo)
    {
        this.ClientId = clientId;
        this.TokenInfo = tokenInfo;
    }

    protected async Task<OAuthTokenInfo> SendObtainRefreshTokenRequestAsync(
        List<KeyValuePair<string, string>> request)
    {
        var response = await SendPostRequestUrlEncodedAsync<OAuthTokenResponse>(
            this.OAuthTokenEndpointUrl,
            request);

        UpdateTokenInfo(response);

        return this.TokenInfo;
    }

    public async Task<OAuthTokenInfo> RefreshAccessTokenAsync()
    {
        var request = GetRefreshAccessTokenRequest();

        var response = await SendPostRequestUrlEncodedAsync<OAuthTokenResponse>(
            this.OAuthTokenEndpointUrl,
            request);

        UpdateTokenInfo(response);

        return this.TokenInfo;
    }

    protected abstract List<KeyValuePair<string, string>> GetRefreshAccessTokenRequest();

    private void UpdateTokenInfo(
        OAuthTokenResponse response)
    {
        if (response.RefreshToken.HasText())
        {
            this.TokenInfo.RefreshToken = response.RefreshToken;
        }

        this.TokenInfo.AccessToken = response.AccessToken;
        this.TokenInfo.AccessTokenExpirationDateTimeUtc = DateTime.UtcNow.AddSeconds(response.ExpiresIn);
        this.TokenInfoUpdated?.Invoke(this.TokenInfo);
    }

    protected async Task<OAuthTokenInfo> EnsureValidAccessTokenAsync(
        DateTime? nowUtc = null)
    {
        // Ensure we have a specified refresh token.
        if (!this.TokenInfo.RefreshToken.HasText())
            throw new Exception($"Token info does not contain a specified refresh token");

        // Calculate an expiration cutoff as the next X minutes.
        var cutoffUtc = (nowUtc ?? DateTime.UtcNow)
            .AddMinutes(this.OAuthTokenPreExpirationCutoffInMinutes);

        // Check for either a missing or expiring access token.
        if (!this.TokenInfo.AccessToken.HasText() ||
            !this.TokenInfo.AccessTokenExpirationDateTimeUtc.HasValue ||
            this.TokenInfo.AccessTokenExpirationDateTimeUtc.Value < cutoffUtc)
        {
            await RefreshAccessTokenAsync();
        }

        return this.TokenInfo;
    }

    protected override async Task BeforeCreateRequestAsync(
        Uri uri)
    {
        if (IsAuthorizationRequired(uri.ToString()))
        {
            await EnsureValidAccessTokenAsync();
        }
    }

    protected override void AddRequestHeaders(
        HttpRequestMessage httpRequest)
    {
        if (IsAuthorizationRequired(httpRequest.RequestUri.ToString()))
        {
            httpRequest.AddBearerTokenAuthorizationHeader(
                this.TokenInfo.AccessToken);
        }
    }

    protected virtual bool IsAuthorizationRequired(
        string url)
    {
        if (url.Equals(this.OAuthTokenEndpointUrl, StringComparison.CurrentCultureIgnoreCase))
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
