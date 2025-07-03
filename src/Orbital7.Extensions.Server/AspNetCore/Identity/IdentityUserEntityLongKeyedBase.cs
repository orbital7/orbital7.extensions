using Microsoft.AspNetCore.Identity;

namespace Orbital7.Extensions.AspNetCore.Identity;

public class IdentityUserEntityLongKeyedBase :
    IdentityUser<long>, IEntity<long>
{
    public DateTime CreatedDateTimeUtc { get; set; }

    public DateTime LastModifiedDateTimeUtc { get; set; }

    protected IdentityUserEntityLongKeyedBase()
    {
        CreatedDateTimeUtc = DateTime.UtcNow;
        LastModifiedDateTimeUtc = CreatedDateTimeUtc;
    }
}

