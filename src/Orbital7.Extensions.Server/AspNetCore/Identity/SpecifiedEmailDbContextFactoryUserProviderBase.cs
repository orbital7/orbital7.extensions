using Microsoft.AspNetCore.Identity;

namespace Orbital7.Extensions.Server.AspNetCore.Identity;

public abstract class SpecifiedEmailDbContextFactoryUserProviderBase<TKey, TUser, TDbContext> :
    IUserProvider
    where TKey : IEquatable<TKey>
    where TUser : IdentityUser<TKey>, IEntity<TKey>
    where TDbContext : DbContext
{
    private readonly IDbContextFactory<TDbContext> _contextFactory;

    protected abstract string UserEmail { get; }

    protected SpecifiedEmailDbContextFactoryUserProviderBase(
        IDbContextFactory<TDbContext> contextFactory)
    {
        _contextFactory = contextFactory;
    }

    public async Task<string?> GetCurrentUserIdAsync(
        CancellationToken cancellationToken = default)
    {
        await using var context = await _contextFactory.CreateDbContextAsync(
            cancellationToken);

        return await SpecifiedEmailDbContextUserProviderBase<TKey, TUser, TDbContext>
            .GetCurrentUserIdFromEmailAsync(
                context,
                this.UserEmail,
                cancellationToken);
    }
}
