using Microsoft.AspNetCore.Http;

namespace Orbital7.Extensions.AspNetCore;

public class HttpContextAccessorUserProvider :
    IUserProvider
{
    private IHttpContextAccessor HttpContextAccessor { get; set; }

    public HttpContextAccessorUserProvider(
        IHttpContextAccessor httpContextAccessor)
    {
        this.HttpContextAccessor = httpContextAccessor;
    }

    public virtual Task<string?> GetCurrentUserIdAsync()
    {
        if (this.HttpContextAccessor?.HttpContext?.User != null)
        {
            var principal = this.HttpContextAccessor.HttpContext.User;
            return Task.FromResult(principal.GetUserId<string?>());
        }

        return Task.FromResult<string?>(null);
    }
}
