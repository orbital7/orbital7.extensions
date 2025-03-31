namespace Orbital7.Extensions.Data;

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
        CreatedDateTimeUtc = DateTime.UtcNow;
        LastModifiedDateTimeUtc = CreatedDateTimeUtc;
    }
}