using Microsoft.AspNetCore.Identity;

namespace Orbital7.Extensions.Server.AspNetCore.Identity;

public abstract class SpecifiedEmailDbContextUserProviderBase<TKey, TUser, TDbContext> :
    IUserProvider
    where TKey : IEquatable<TKey>
    where TUser : IdentityUser<TKey>, IEntity<TKey>
    where TDbContext : DbContext
{
    private readonly TDbContext _context;

    protected abstract string UserEmail { get; }

    protected SpecifiedEmailDbContextUserProviderBase(
        TDbContext context)
    {
        _context = context;
    }

    public async Task<string?> GetCurrentUserIdAsync(
        CancellationToken cancellationToken = default)
    {
        return await GetCurrentUserIdFromEmailAsync(
            _context, 
            this.UserEmail,
            cancellationToken);
    }

    internal static async Task<string?> GetCurrentUserIdFromEmailAsync(
        TDbContext context,
        string email,
        CancellationToken cancellationToken)
    {
        var userId = await context.Set<TUser>()
            .Where(x => x.Email == email)
            .Select(x => x.Id)
            .FirstOrDefaultAsync(
                cancellationToken);

        if (userId == null ||
            userId.Equals(default))
        {
            return null;
        }
        else
        {
            return userId.ToString();
        }
    }
}

