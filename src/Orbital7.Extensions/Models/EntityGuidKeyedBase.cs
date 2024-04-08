using System.ComponentModel.DataAnnotations;

namespace System;

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
        this.Id = GuidFactory.NextSequential();
        this.CreatedDateTimeUtc = DateTime.UtcNow;
        this.LastModifiedDateTimeUtc = this.CreatedDateTimeUtc;
    }
}
