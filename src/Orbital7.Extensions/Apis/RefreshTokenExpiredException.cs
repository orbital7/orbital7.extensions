namespace Orbital7.Extensions.Apis;

public class RefreshTokenExpiredException :
    Exception
{
    public RefreshTokenExpiredException()
        : base("Refresh token is expired")
    {
    }
}
