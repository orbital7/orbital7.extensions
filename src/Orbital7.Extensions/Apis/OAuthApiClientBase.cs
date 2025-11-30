namespace Orbital7.Extensions.Apis;

public abstract class OAuthApiClientBase<TTokenInfo> :
    ApiClient, IOAuthApiClient
    where TTokenInfo : TokenInfo
{
    private IServiceProvider ServiceProvider { get; set; }

    public TTokenInfo TokenInfo { get; private set; }

    protected abstract string OAuthTokenEndpointUrl { get; }

    protected virtual int OAuthAccessTokenPreExpirationBufferInMinutes => 10;

    private Func<IServiceProvider, TTokenInfo, Task>? OnTokenInfoUpdated { get; set; }

    protected OAuthApiClientBase(
        IServiceProvider serviceProvider,
        IHttpClientFactory httpClientFactory,
        TTokenInfo tokenInfo,
        Func<IServiceProvider, TTokenInfo, Task>? onTokenInfoUpdated = null,
        string? httpClientName = null) :
        base(httpClientFactory, httpClientName)
    {
        this.ServiceProvider = serviceProvider;
        this.TokenInfo = tokenInfo;
        this.OnTokenInfoUpdated = onTokenInfoUpdated;
    }

    public abstract string GetAuthorizationUrl(
        string? state = null);

    public async Task<bool> EnsureValidAccessTokenAsync(
        CancellationToken cancellationToken = default,
        DateTime? nowUtc = null)
    {
        // Check for an invalid access token.
        if (!this.TokenInfo.IsAccessTokenValid(
            this.OAuthAccessTokenPreExpirationBufferInMinutes,
            nowUtc))
        {
            var refreshTokenStatus = this.TokenInfo.GetRefreshTokenStatus(
                this.OAuthAccessTokenPreExpirationBufferInMinutes,
                nowUtc);

            switch (refreshTokenStatus)
            {
                case TokenStatus.Valid:
                    await RefreshTokenAsync(
                        this.TokenInfo.RefreshToken!,
                        cancellationToken);
                    return true;

                case TokenStatus.Empty:
                    await GetTokenAsync(cancellationToken);
                    return true;

                case TokenStatus.Expired:
                    throw new RefreshTokenExpiredException();
            }
        }

        return false;
    }

    protected abstract List<KeyValuePair<string, string>> CreateGetTokenRequest();

    protected abstract List<KeyValuePair<string, string>> CreateRefreshTokenRequest(
        string refreshToken);

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
    protected virtual async Task UpdateTokenInfoAsync(
        OAuthTokenResponse response)
    {
        // Validate and record access token info.
        if (response.AccessToken.HasText())
        {
            this.TokenInfo.AccessToken = response.AccessToken;
            this.TokenInfo.AccessTokenExpirationDateTimeUtc = response.ExpiresIn.HasValue ?
                DateTime.UtcNow
                    .RoundDownToStartOfSecond()
                    .AddSeconds(response.ExpiresIn.Value) :
                null;
        }
        else
        {
            throw new Exception("Response does not contain an access token");
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

    private async Task<TTokenInfo> GetTokenAsync(
        CancellationToken cancellationToken)
    {
        var request = CreateGetTokenRequest();

        return await SendOAuthTokenRequestAsync(
            request,
            cancellationToken);
    }

    private async Task<TTokenInfo> RefreshTokenAsync(
        string refreshToken,
        CancellationToken cancellationToken)
    {
        var request = CreateRefreshTokenRequest(refreshToken);

        return await SendOAuthTokenRequestAsync(
            request,
            cancellationToken);
    }

    private async Task<TTokenInfo> SendOAuthTokenRequestAsync(
        List<KeyValuePair<string, string>> request,
        CancellationToken cancellationToken)
    {
        var response = await SendPostRequestUrlEncodedAsync<OAuthTokenResponse>(
            this.OAuthTokenEndpointUrl,
            request,
            cancellationToken);

        await UpdateTokenInfoAsync(response);

        return this.TokenInfo;
    }
}
