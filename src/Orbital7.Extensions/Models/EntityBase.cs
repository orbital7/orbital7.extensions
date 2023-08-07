
using System.ComponentModel.DataAnnotations;

namespace System;

public abstract class EntityBase :
    IEntity
{
    [Key]
    public Guid Id { get; set; }

    public DateTime CreatedDateTimeUtc { get; set; }

    public DateTime LastModifiedDateTimeUtc { get; set; }

    protected EntityBase()
    {
        this.Id = SequentialGuidFactory.NewGuid();
        this.CreatedDateTimeUtc = DateTime.UtcNow;
        this.LastModifiedDateTimeUtc = this.CreatedDateTimeUtc;
    }
}
