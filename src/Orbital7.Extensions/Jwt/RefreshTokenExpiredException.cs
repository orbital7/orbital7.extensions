namespace Orbital7.Extensions.Jwt;

public class RefreshTokenExpiredException :
    Exception
{
    public const string MESSAGE = "Refresh token is expired";

    public RefreshTokenExpiredException() :
        base(MESSAGE)
    {

    }
}
