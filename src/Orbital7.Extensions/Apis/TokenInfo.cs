namespace Orbital7.Extensions.Apis;

public class TokenInfo
{
    public string? RefreshToken { get; set; }

    public DateTime? RefreshTokenExpirationDateTimeUtc { get; set; }

    public string? AccessToken { get; set; }

    public DateTime? AccessTokenExpirationDateTimeUtc { get; set; }
}
