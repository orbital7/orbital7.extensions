namespace Orbital7.Extensions.Apis;

public class TokenInfo
{
    public string? RefreshToken { get; set; }

    public DateTime? RefreshTokenExpirationDateTimeUtc { get; set; }

    public string? AccessToken { get; set; }

    public DateTime? AccessTokenExpirationDateTimeUtc { get; set; }

    public TokenStatus GetRefreshTokenStatus(
        int? preExpirationBufferInMinutes = null,
        DateTime? nowUtc = null)
    {
        return GetTokenStatus(
            this.RefreshToken,
            this.RefreshTokenExpirationDateTimeUtc,
            preExpirationBufferInMinutes,
            nowUtc);
    }

    [MemberNotNullWhen(true, nameof(AccessToken))]
    public bool IsAccessTokenValid(
        int? preExpirationBufferInMinutes = null,
        DateTime? nowUtc = null)
    {
        var tokenStatus = GetAccessTokenStatus(
            preExpirationBufferInMinutes,
            nowUtc);
        
        return tokenStatus == TokenStatus.Valid;
    }

    public TokenStatus GetAccessTokenStatus(
        int? preExpirationBufferInMinutes = null,
        DateTime? nowUtc = null)
    {
        return GetTokenStatus(
            this.AccessToken,
            this.AccessTokenExpirationDateTimeUtc,
            preExpirationBufferInMinutes,
            nowUtc);
    }

    private TokenStatus GetTokenStatus(
        string? token,
        DateTime? tokenExpirationDateTimeUtc,
        int? preExpirationBufferInMinutes,
        DateTime? nowUtc = null)
    {
        // Calculate an adjusted expiration date/time as within the next X buffer minutes.
        var adjustedExpirationDateTimeUtc = (nowUtc ?? DateTime.UtcNow)
            .RoundDownToStartOfSecond()
            .ToUniversalTime()
            .AddMinutes(preExpirationBufferInMinutes ?? 0);

        // Validate.
        if (!token.HasText())
        {
            return TokenStatus.Empty;
        }
        else if (tokenExpirationDateTimeUtc.HasValue &&
            tokenExpirationDateTimeUtc.Value.ToUniversalTime() <= adjustedExpirationDateTimeUtc)
        {
            return TokenStatus.Expired;
        }
        else
        {
            return TokenStatus.Valid;
        }
    }
}
