using Microsoft.AspNetCore.Http;

namespace Orbital7.Extensions.AspNetCore;

public class HttpContextAccessorUserProvider(
    IHttpContextAccessor httpContextAccessor) :
    IUserProvider
{
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

    public virtual Task<string?> GetCurrentUserIdAsync(
        CancellationToken cancellationToken = default)
    {
        if (_httpContextAccessor?.HttpContext?.User != null)
        {
            var principal = _httpContextAccessor.HttpContext.User;
            return Task.FromResult(principal.GetUserId<string?>());
        }

        return Task.FromResult<string?>(null);
    }
}
