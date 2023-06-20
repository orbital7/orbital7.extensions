
using System.ComponentModel.DataAnnotations;

namespace System;

public abstract class EntityBase :
    IEntity
{
    [Key]
    public Guid Id { get; set; }

    public DateTime CreatedDateUtc { get; set; }

    public DateTime LastModifiedDateUtc { get; set; }

    protected EntityBase()
    {
        this.Id = SequentialGuidFactory.NewGuid();
        this.CreatedDateUtc = DateTime.UtcNow;
        this.LastModifiedDateUtc = this.CreatedDateUtc;
    }
}
