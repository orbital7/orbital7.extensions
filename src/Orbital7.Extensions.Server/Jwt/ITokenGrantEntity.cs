using Microsoft.AspNetCore.Identity;

namespace Orbital7.Extensions.Jwt;

public interface ITokenGrantEntity<TKey, TUser> : 
    IEntity<TKey>
    where TKey : IEquatable<TKey>
    where TUser : IdentityUser<TKey>
{
    TKey UserId { get; set; }

    TUser? User { get; set; }

    string? Description { get; set; }

    string? RefreshToken { get; set; }

    DateTime? ExpirationDateTimeUtc { get; set; }

    DateTime? LastRefreshedDateTimeUtc { get; set; }
}
