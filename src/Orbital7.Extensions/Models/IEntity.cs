namespace System;

public interface IEntity
{
    Guid Id { get; set; }

    DateTime CreatedDateUtc { get; set; }

    DateTime LastModifiedDateUtc { get; set; }
}
