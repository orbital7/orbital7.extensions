namespace Orbital7.Extensions.Apis;

public abstract class OAuthApiClientBase<TTokenInfo> :
    ApiClient, IOAuthApiClient
    where TTokenInfo : TokenInfo
{
    private readonly Func<IServiceProvider, TTokenInfo, CancellationToken, Task>? _onTokenInfoUpdated;

    public TTokenInfo TokenInfo { get; private set; }

    protected IServiceProvider ServiceProvider { get; init; }

    protected abstract string OAuthTokenEndpointUrl { get; }

    protected virtual int AccessTokenPreExpirationBufferInMinutes => 10;

    protected OAuthApiClientBase(
        IServiceProvider serviceProvider,
        IHttpClientFactory httpClientFactory,
        TTokenInfo tokenInfo,
        Func<IServiceProvider, TTokenInfo, CancellationToken, Task>? onTokenInfoUpdated = null,
        string? httpClientName = null) :
        base(httpClientFactory, httpClientName)
    {
        ArgumentNullException.ThrowIfNull(serviceProvider);
        ArgumentNullException.ThrowIfNull(httpClientFactory);
        ArgumentNullException.ThrowIfNull(tokenInfo);

        this.ServiceProvider = serviceProvider;
        this.TokenInfo = tokenInfo;

        _onTokenInfoUpdated = onTokenInfoUpdated;

    }

    public abstract string GetAuthorizationUrl(
        string? state = null);

    public async Task<string> EnsureValidAccessTokenAsync(
        CancellationToken cancellationToken = default,
        DateTime? nowUtc = null)
    {
        // Check for an invalid access token.
        if (!this.TokenInfo.IsAccessTokenValid(
            this.AccessTokenPreExpirationBufferInMinutes,
            nowUtc))
        {
            var refreshTokenStatus = this.TokenInfo.GetRefreshTokenStatus(
                this.AccessTokenPreExpirationBufferInMinutes,
                nowUtc);

            switch (refreshTokenStatus)
            {
                case TokenStatus.Valid:
                    await RefreshTokenAsync(
                        this.TokenInfo.RefreshToken!,
                        cancellationToken);
                    break;

                case TokenStatus.Empty:
                    await GetTokenAsync(cancellationToken);
                    break;

                case TokenStatus.Expired:
                    throw new RefreshTokenExpiredException();
            }
        }

        // This should never happen, but final check to ensure access token is not null.
        ArgumentNullException.ThrowIfNull(
            this.TokenInfo.AccessToken, 
            nameof(this.TokenInfo.AccessToken));

        return this.TokenInfo.AccessToken;
    }

    protected abstract Task<List<KeyValuePair<string, string>>> CreateGetTokenRequestAsync();

    protected abstract List<KeyValuePair<string, string>> CreateRefreshTokenRequest(
        string refreshToken);

    protected override async Task BeforeCreateRequestAsync(
        Uri uri,
        CancellationToken cancellationToken)
    {
        if (IsAuthorizationRequired(uri))
        {
            await EnsureValidAccessTokenAsync(
                cancellationToken: cancellationToken);
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

    protected virtual void UpdateTokenInfo(
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
    }

    private async Task<TTokenInfo> GetTokenAsync(
        CancellationToken cancellationToken)
    {
        var request = await CreateGetTokenRequestAsync();

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
            cancellationToken: cancellationToken);

        // Update the token info.
        UpdateTokenInfo(response);

        // Handle the token info updated event.
        await NotifiyTokenInfoUpdatedAsync(
            cancellationToken);

        return this.TokenInfo;
    }

    private async Task NotifiyTokenInfoUpdatedAsync(
        CancellationToken cancellationToken)
    {
        if (_onTokenInfoUpdated != null)
        {
            await _onTokenInfoUpdated.Invoke(
                this.ServiceProvider,
                this.TokenInfo,
                cancellationToken);
        }
    }
}
