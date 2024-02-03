namespace Orbital7.Extensions.Integrations;

public class OAuthTokenInfo
{
    public string RefreshToken { get; set; }

    public string AccessToken { get; set; }

    public DateTime? AccessTokenExpirationDateTimeUtc { get; set; }
}
