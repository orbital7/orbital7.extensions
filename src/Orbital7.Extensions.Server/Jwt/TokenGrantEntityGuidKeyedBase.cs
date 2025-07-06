using Microsoft.AspNetCore.Identity;

namespace Orbital7.Extensions.Jwt;

public abstract class TokenGrantEntityGuidKeyedBase<TUser> :
    EntityGuidKeyedBase, 
    ITokenGrantEntity<Guid, TUser>
    where TUser : IdentityUser<Guid>, IEntity<Guid>
{
    public Guid UserId { get; set; }

    public TUser? User { get; set; }

    [StringLength(128)]
    public string? Description { get; set; }

    // TODO: Make required.
    [StringLength(128)]
    public string? RefreshToken { get; set; }

    // TODO: Make non-nullable.
    public DateTime? LastRefreshedDateTimeUtc { get; set; }

    public DateTime? ExpirationDateTimeUtc { get; set; }
}
