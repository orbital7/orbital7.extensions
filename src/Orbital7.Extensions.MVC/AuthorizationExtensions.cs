using System;
using System.Security.Claims;

namespace Microsoft.AspNetCore.Authorization
{
    public static class AuthorizationExtensions
    {
        public static Guid GetUserId(this ClaimsPrincipal principal)
        {
            if (principal == null)
                throw new ArgumentNullException(nameof(principal));

            var first = principal.FindFirst(ClaimTypes.NameIdentifier);
            if (first != null)
                return Guid.Parse(first.Value);
            else
                return Guid.Empty;
        }
    }
}
