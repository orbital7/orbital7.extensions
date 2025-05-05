using Microsoft.AspNetCore.Identity;

namespace Orbital7.Extensions.EntityFrameworkCore.AspNetCoreIdentity;

public class IdentityUserEntityLongKeyedBase :
    IdentityUser<long>, IEntity<long>
{
    public DateTime CreatedDateTimeUtc { get; set; }

    public DateTime LastModifiedDateTimeUtc { get; set; }

    protected IdentityUserEntityLongKeyedBase()
    {
        this.CreatedDateTimeUtc = DateTime.UtcNow;
        this.LastModifiedDateTimeUtc = this.CreatedDateTimeUtc;
    }
}

