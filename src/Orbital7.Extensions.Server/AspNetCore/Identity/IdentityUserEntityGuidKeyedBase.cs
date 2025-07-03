using Microsoft.AspNetCore.Identity;

namespace Orbital7.Extensions.AspNetCore.Identity;

public abstract class IdentityUserEntityGuidKeyedBase :
    IdentityUser<Guid>, IEntity<Guid>
{
    public DateTime CreatedDateTimeUtc { get; set; }

    public DateTime LastModifiedDateTimeUtc { get; set; }

    protected IdentityUserEntityGuidKeyedBase()
    {
        Id = GuidFactory.NextSequential();
        CreatedDateTimeUtc = DateTime.UtcNow;
        LastModifiedDateTimeUtc = CreatedDateTimeUtc;
    }
}
