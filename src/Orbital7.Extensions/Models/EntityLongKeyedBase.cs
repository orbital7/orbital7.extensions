using System.ComponentModel.DataAnnotations;

namespace System;

// NOTE: This *should* be used with something like Sqids to 
// obfuscate IDs: https://github.com/sqids/sqids-dotnet
public abstract class EntityLongKeyedBase :
    IEntity<long>
{
    [Key]
    public long Id { get; set; }

    public DateTime CreatedDateTimeUtc { get; set; }

    public DateTime LastModifiedDateTimeUtc { get; set; }

    protected EntityLongKeyedBase()
    {
        this.CreatedDateTimeUtc = DateTime.UtcNow;
        this.LastModifiedDateTimeUtc = this.CreatedDateTimeUtc;
    }
}