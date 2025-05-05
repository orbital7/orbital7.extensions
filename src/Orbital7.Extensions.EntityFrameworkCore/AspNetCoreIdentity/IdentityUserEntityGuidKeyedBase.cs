using Microsoft.AspNetCore.Identity;

namespace Orbital7.Extensions.EntityFrameworkCore.AspNetCoreIdentity;

public abstract class IdentityUserEntityGuidKeyedBase :
    IdentityUser<Guid>, IEntity<Guid>
{
    public DateTime CreatedDateTimeUtc { get; set; }

    public DateTime LastModifiedDateTimeUtc { get; set; }

    protected IdentityUserEntityGuidKeyedBase()
    {
        this.Id = GuidFactory.NextSequential();
        this.CreatedDateTimeUtc = DateTime.UtcNow;
        this.LastModifiedDateTimeUtc = this.CreatedDateTimeUtc;
    }
}
