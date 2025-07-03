using Microsoft.AspNetCore.Identity;

namespace Orbital7.Extensions.AspNetCore.Identity;

public class SpecifiedUserProvider<TKey, TUser, TDbContext> :
    IUserProvider
    where TKey : IEquatable<TKey>
    where TUser : IdentityUser<TKey>, IEntity<TKey>
    where TDbContext : DbContext
{
    private readonly TDbContext _context;
    private TKey? _currentUserId;

    public SpecifiedUserProvider(
        TDbContext context)
    {
        _context = context;
    }

    public Task<string?> GetCurrentUserIdAsync()
    {
        return Task.FromResult(_currentUserId?.ToString());
    }

    public virtual async Task SetCurrentUserAsync(
        string username)
    {
        var userId = await _context.Set<TUser>()
            .Where(x => x.UserName == username)
            .Select(x => x.Id)
            .FirstOrDefaultAsync();

        if (userId != null)
        {
            _currentUserId = userId;
        }
        else
        {
            throw new Exception($"The specified user \"{username}\" was not found");
        }
    }

    public static async Task SetCurrentUserAsync(
        IServiceProvider serviceProvider,
        string username)
    {
        var userProvider = (SpecifiedUserProvider<TKey, TUser, TDbContext>)
            serviceProvider.GetRequiredService<IUserProvider>();

        await userProvider.SetCurrentUserAsync(username);
    }
}
