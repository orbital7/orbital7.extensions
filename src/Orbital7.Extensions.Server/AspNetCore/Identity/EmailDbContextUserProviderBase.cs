using Microsoft.AspNetCore.Identity;

namespace Orbital7.Extensions.Server.AspNetCore.Identity;

public abstract class EmailDbContextUserProviderBase<TKey, TUser, TDbContext> :
    IUserProvider
    where TKey : IEquatable<TKey>
    where TUser : IdentityUser<TKey>, IEntity<TKey>
    where TDbContext : DbContext
{
    private readonly TDbContext _context;

    protected EmailDbContextUserProviderBase(
        TDbContext context)
    {
        _context = context;
    }

    public async Task<string?> GetCurrentUserIdAsync(
        CancellationToken cancellationToken = default)
    {
        return await GetCurrentUserIdFromEmailAsync(
            _context,
            await GetUserEmailAsync(cancellationToken),
            cancellationToken);
    }

    protected abstract Task<string?> GetUserEmailAsync(
        CancellationToken cancellationToken = default);

    internal static async Task<string?> GetCurrentUserIdFromEmailAsync(
        TDbContext context,
        string? email,
        CancellationToken cancellationToken)
    {
        if (email.HasText())
        {
            var userId = await context.Set<TUser>()
                .Where(x => x.Email == email)
                .Select(x => x.Id)
                .FirstOrDefaultAsync(
                    cancellationToken);

            if (userId != null &&
                !userId.Equals(default))
            {
                return userId.ToString();
            }
        }

        return null;
    }
}

