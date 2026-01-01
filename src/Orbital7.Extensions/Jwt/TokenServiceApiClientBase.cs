using Orbital7.Extensions.Apis;

namespace Orbital7.Extensions.Jwt;

public abstract class TokenServiceApiClientBase<TTokenInfo> :
    ApiClient, ITokenServiceApiClient<TTokenInfo>
    where TTokenInfo : TokenInfo, new()
{
    private readonly Func<IServiceProvider, CancellationToken, Task<TTokenInfo>>? _onTokenInfoEmpty;
    private readonly Func<IServiceProvider, TTokenInfo, CancellationToken, Task>? _onTokenInfoUpdated;

    public TTokenInfo TokenInfo { get; private set; }

    protected IServiceProvider ServiceProvider { get; init; }

    protected virtual int AccessTokenPreExpirationBufferInMinutes => 10;

    protected abstract string GetTokenEndpointUrl { get; }

    protected abstract string RefreshTokenEndpointUrl { get; }

    protected abstract string RevokeTokenEndpointUrl { get; }

    protected TokenServiceApiClientBase(
        IServiceProvider serviceProvider,
        IHttpClientFactory httpClientFactory,
        TTokenInfo tokenInfo,
        Func<IServiceProvider, CancellationToken, Task<TTokenInfo>>? onTokenInfoEmpty = null,
        Func<IServiceProvider, TTokenInfo, CancellationToken, Task>? onTokenInfoUpdated = null,
        string? httpClientName = null) :
        base(
            httpClientFactory,
            httpClientName: httpClientName)
    {
        ArgumentNullException.ThrowIfNull(serviceProvider);
        ArgumentNullException.ThrowIfNull(httpClientFactory);
        ArgumentNullException.ThrowIfNull(tokenInfo);

        this.ServiceProvider = serviceProvider;
        this.TokenInfo = tokenInfo;

        _onTokenInfoEmpty = onTokenInfoEmpty;
        _onTokenInfoUpdated = onTokenInfoUpdated;
    }

    public async Task<TTokenInfo> GetTokenAsync(
        GetTokenInput input,
        CancellationToken cancellationToken = default)
    {
        var tokenInfo = await this.SendPostRequestAsync<GetTokenInput, TTokenInfo>(
            this.GetTokenEndpointUrl,
            input,
            cancellationToken: cancellationToken);

        await UpdateTokenInfoAsync(
            tokenInfo,
            cancellationToken);

        return tokenInfo;
    }

    public async Task<RevokedTokenInfo?> RevokeTokenAsync(
        RevokeTokenInput input,
        CancellationToken cancellationToken = default)
    {
        var revokedTokenInfo = await this.SendPostRequestAsync<RevokeTokenInput, RevokedTokenInfo?>(
            this.RevokeTokenEndpointUrl,
            input,
            cancellationToken: cancellationToken);

        await UpdateTokenInfoAsync(
            new TTokenInfo(),
            cancellationToken);

        return revokedTokenInfo;
    }

    public async Task<string> EnsureValidAccessTokenAsync(
        CancellationToken cancellationToken = default,
        DateTime? nowUtc = null)
    {
        // If token info is empty, handle it.
        if (!this.TokenInfo.AccessToken.HasText() &&
            _onTokenInfoEmpty != null)
        {
            var updatedTokenInfo = await _onTokenInfoEmpty.Invoke(
                this.ServiceProvider,
                cancellationToken);

            await UpdateTokenInfoAsync(
                updatedTokenInfo,
                cancellationToken);
        }

        // Check for an invalid access token.
        if (!this.TokenInfo.IsAccessTokenValid(
            this.AccessTokenPreExpirationBufferInMinutes,
            nowUtc))
        {
            var refreshTokenStatus = this.TokenInfo.GetRefreshTokenStatus(
                this.AccessTokenPreExpirationBufferInMinutes,
                nowUtc);

            if (refreshTokenStatus == TokenStatus.Expired)
            {
                throw new RefreshTokenExpiredException();
            }
            else if (refreshTokenStatus == TokenStatus.Empty)
            {
                throw new Exception("Refresh token is empty");
            }
            else
            {
                await RefreshTokenAsync(
                    this.TokenInfo,
                    cancellationToken);
            }
        }

        // This should never happen, but final check to ensure access token is not null.
        ArgumentNullException.ThrowIfNull(this.TokenInfo.AccessToken, nameof(this.TokenInfo.AccessToken));

        return this.TokenInfo.AccessToken;
    }

    protected override void AddRequestHeaders(
        HttpRequestMessage httpRequest)
    {
        if (IsAuthorizationRequired(httpRequest.RequestUri))
        {
            httpRequest.AddBearerTokenAuthorizationHeader(
                this.TokenInfo?.AccessToken);
        }
    }

    protected virtual bool IsAuthorizationRequired(
        Uri? uri)
    {
        var uriString = uri?.ToString();

        if (!uriString.HasText() ||
            uriString.Equals(this.GetTokenEndpointUrl, StringComparison.OrdinalIgnoreCase) ||
            uriString.Equals(this.RefreshTokenEndpointUrl, StringComparison.OrdinalIgnoreCase) ||
            uriString.Equals(this.RevokeTokenEndpointUrl, StringComparison.OrdinalIgnoreCase))
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    protected override Exception CreateUnsuccessfulResponseException(
        HttpResponseMessage httpResponse,
        string responseBody)
    {
        return CreateProblemDetailsResponseException(
            httpResponse,
            responseBody);
    }

    protected Exception CreateProblemDetailsResponseException(
        HttpResponseMessage httpResponse,
        string responseBody)
    {
        ProblemDetailsResponse problemDetailsResponse;

        if (responseBody.HasText())
        {
            if (responseBody.StartsWith("{"))
            {
                try
                {
                    problemDetailsResponse = JsonSerializationHelper.DeserializeFromJson<ProblemDetailsResponse>(responseBody);
                }
                catch (Exception ex)
                {
                    problemDetailsResponse = new ProblemDetailsResponse()
                    {
                        Title = "Unable to deserialize error response JSON.",
                        Detail = ex.Message,
                        Status = (int)httpResponse.StatusCode,
                        Extensions = new Dictionary<string, object?>()
                        {
                            { "ResponseBody", responseBody },
                        }
                    };
                }
            }
            else
            {
                problemDetailsResponse = new ProblemDetailsResponse()
                {
                    Title = "Error response is not JSON.",
                    Detail = responseBody,
                    Status = (int)httpResponse.StatusCode,
                };
            }
        }
        else
        {
            problemDetailsResponse = new ProblemDetailsResponse()
            {
                Title = "Error response is empty.",
                Detail = httpResponse.ReasonPhrase,
                Status = (int)httpResponse.StatusCode,
            };
        }

        return new ProblemDetailsException(problemDetailsResponse);
    }

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

    private async Task UpdateTokenInfoAsync(
        TTokenInfo tokenInfo,
        CancellationToken cancellationToken)
    {
        this.TokenInfo.Import(tokenInfo);

        if (_onTokenInfoUpdated != null)
        {
            await _onTokenInfoUpdated.Invoke(
                this.ServiceProvider, 
                tokenInfo,
                cancellationToken);
        }
    }

    private async Task RefreshTokenAsync(
        TTokenInfo tokenInfo,
        CancellationToken cancellationToken)
    {
        var updatedTokenInfo = await this.SendPostRequestAsync<RefreshTokenInput, TTokenInfo>(
            this.RefreshTokenEndpointUrl,
            new RefreshTokenInput()
            {
                AccessToken = tokenInfo.AccessToken,
                RefreshToken = tokenInfo.RefreshToken,
            },
            cancellationToken: cancellationToken);

        await UpdateTokenInfoAsync(
            updatedTokenInfo,
            cancellationToken);
    }
}
