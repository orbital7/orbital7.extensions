namespace Orbital7.Extensions.Data;

// NOTE: This uses NewId to create sequential GUIDs; relevant links:
// * https://masstransit.io/documentation/patterns/newid#newid
// * https://github.com/phatboyg/NewId
// * https://andrewlock.net/generating-sortable-guids-using-newid/
public abstract class EntityGuidKeyedBase :
    IEntity<Guid>
{
    [Key]
    public Guid Id { get; set; }

    public DateTime CreatedDateTimeUtc { get; set; }

    public DateTime LastModifiedDateTimeUtc { get; set; }

    protected EntityGuidKeyedBase()
    {
        Id = GuidFactory.NextSequential();
        CreatedDateTimeUtc = DateTime.UtcNow;
        LastModifiedDateTimeUtc = CreatedDateTimeUtc;
    }
}
