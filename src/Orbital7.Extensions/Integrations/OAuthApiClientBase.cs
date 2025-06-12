namespace Orbital7.Extensions.Integrations;

public abstract class OAuthApiClientBase :
    ApiClient
{
    private IServiceProvider ServiceProvider { get; set; }

    protected string ClientId { get; private set; }

    public TokenInfo TokenInfo { get; private set; }

    protected abstract string OAuthTokenEndpointUrl { get; }

    protected virtual int OAuthAccessTokenPreExpirationCutoffInMinutes => 10;

    private Func<IServiceProvider, TokenInfo, Task>? OnTokenInfoUpdated { get; set; }

    protected OAuthApiClientBase(
        IServiceProvider serviceProvider,
        IHttpClientFactory httpClientFactory,
        string clientId,
        TokenInfo tokenInfo,
        Func<IServiceProvider, TokenInfo, Task>? onTokenInfoUpdated = null) :
        base(httpClientFactory)
    {
        this.ServiceProvider = serviceProvider;
        this.ClientId = clientId;
        this.TokenInfo = tokenInfo;
        this.OnTokenInfoUpdated = onTokenInfoUpdated;
    }

    protected async Task<TokenInfo> SendObtainTokenRequestAsync(
        List<KeyValuePair<string, string>> request,
        CancellationToken cancellationToken = default)
    {
        var response = await SendPostRequestUrlEncodedAsync<OAuthTokenResponse>(
            this.OAuthTokenEndpointUrl,
            request,
            cancellationToken);

        await UpdateTokenInfoAsync(response);

        return this.TokenInfo;
    }

    private async Task<TokenInfo> RefreshTokenAsync(
        CancellationToken cancellationToken)
    {
        var request = GetRefreshTokenRequest();

        var response = await SendPostRequestUrlEncodedAsync<OAuthTokenResponse>(
            this.OAuthTokenEndpointUrl,
            request,
            cancellationToken);

        await UpdateTokenInfoAsync(response);

        return this.TokenInfo;
    }

    protected abstract List<KeyValuePair<string, string>> GetRefreshTokenRequest();

    private async Task UpdateTokenInfoAsync(
        OAuthTokenResponse response)
    {
        // Validate abnd record access token info.
        if (response.AccessToken.HasText() &&
            response.ExpiresIn.HasValue)
        {
            this.TokenInfo.AccessToken = response.AccessToken;
            this.TokenInfo.AccessTokenExpirationDateTimeUtc = DateTime.UtcNow.AddSeconds(response.ExpiresIn.Value);
        }
        else
        {
            throw new Exception("Response does not contain access token and/or expiration time");
        }

        // Sometimes we get a null refresh token, so only overwrite if we were returned one.
        if (response.RefreshToken.HasText())
        {
            this.TokenInfo.RefreshToken = response.RefreshToken;
        }

        // Handle the token info updated event.
        if (this.OnTokenInfoUpdated != null)
        {
            await this.OnTokenInfoUpdated.Invoke(this.ServiceProvider, this.TokenInfo);
        }
    }

    protected async Task<TokenInfo> EnsureValidAccessTokenAsync(
        CancellationToken cancellationToken,
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
            await RefreshTokenAsync(cancellationToken);
        }

        return this.TokenInfo;
    }

    protected override async Task BeforeCreateRequestAsync(
        Uri uri,
        CancellationToken cancellationToken)
    {
        if (IsAuthorizationRequired(uri))
        {
            await EnsureValidAccessTokenAsync(
                cancellationToken);
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
        Uri? uri)
    {
        if (uri == null || 
            uri.ToString().Equals(this.OAuthTokenEndpointUrl, StringComparison.OrdinalIgnoreCase))
        {
            return false;
        }
        else
        {
            return true;
        }
    }
}
