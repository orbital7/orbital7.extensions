using Microsoft.IdentityModel.Tokens;

namespace Orbital7.Extensions.Jwt;

internal static class TokenServiceHelper
{
    public static SymmetricSecurityKey GetSigningKey(
        string signingKey)
    {
        return new SymmetricSecurityKey(Encoding.UTF8.GetBytes(signingKey));
    }
}
