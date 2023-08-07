namespace System;

public interface IEntity
{
    Guid Id { get; set; }

    DateTime CreatedDateTimeUtc { get; set; }

    DateTime LastModifiedDateTimeUtc { get; set; }
}
