namespace Orbital7.Extensions;

public interface IUserProvider
{
    Task<string?> GetCurrentUserIdAsync();
}
