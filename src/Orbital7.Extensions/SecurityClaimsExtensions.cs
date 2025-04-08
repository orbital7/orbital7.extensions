using System.Security.Claims;

namespace Orbital7.Extensions;

public static class SecurityClaimsExtensions
{
    public static Guid GetUserId(
        this ClaimsPrincipal principal)
    {
        if (principal == null)
            throw new ArgumentNullException(nameof(principal));

        var first = principal.FindFirst(ClaimTypes.NameIdentifier);

        if (first != null)
        {
            return Guid.Parse(first.Value);
        }
        else
        {
            return Guid.Empty;
        }
    }

    public static string? GetUsername(
        this ClaimsPrincipal principal)
    {
        if (principal == null)
            throw new ArgumentNullException(nameof(principal));

        var first = principal.FindFirst(ClaimTypes.Name);

        if (first != null)
        {
            return first.Value;
        }
        else
        {
            return null;
        }
    }
}
