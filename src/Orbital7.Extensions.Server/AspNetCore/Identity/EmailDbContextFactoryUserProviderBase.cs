using Microsoft.AspNetCore.Identity;

namespace Orbital7.Extensions.Server.AspNetCore.Identity;

public abstract class EmailDbContextFactoryUserProviderBase<TKey, TUser, TDbContext> :
    IUserProvider
    where TKey : IEquatable<TKey>
    where TUser : IdentityUser<TKey>, IEntity<TKey>
    where TDbContext : DbContext
{
    private readonly IDbContextFactory<TDbContext> _contextFactory;

    protected EmailDbContextFactoryUserProviderBase(
        IDbContextFactory<TDbContext> contextFactory)
    {
        _contextFactory = contextFactory;
    }

    public async Task<string?> GetCurrentUserIdAsync(
        CancellationToken cancellationToken = default)
    {
        await using var context = await _contextFactory.CreateDbContextAsync(
            cancellationToken);

        return await EmailDbContextUserProviderBase<TKey, TUser, TDbContext>
            .GetCurrentUserIdFromEmailAsync(
                context,
                await GetUserEmailAsync(cancellationToken),
                cancellationToken);
    }

    protected abstract Task<string?> GetUserEmailAsync(
        CancellationToken cancellationToken = default);
}
