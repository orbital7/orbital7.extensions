using Microsoft.AspNetCore.Identity;

namespace Orbital7.Extensions.Jwt;

public abstract class TokenGrantEntityLongKeyedBase<TUser> :
    EntityLongKeyedBase, 
    ITokenGrantEntity<long, TUser>
    where TUser : IdentityUser<long>, IEntity<long>
{
    public long UserId { get; set; }

    public TUser? User { get; set; }

    [StringLength(128)]
    public string? Description { get; set; }

    [StringLength(128)]
    public string? RefreshToken { get; set; }

    public DateTime? ExpirationDateTimeUtc { get; set; }

    public DateTime? LastRefreshedDateTimeUtc { get; set; }
}
