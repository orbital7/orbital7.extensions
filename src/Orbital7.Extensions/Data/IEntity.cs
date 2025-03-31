namespace Orbital7.Extensions.Data;

public interface IEntity<TKey> :
    IEntity
    where TKey : IEquatable<TKey>
{
    TKey Id { get; set; }
}

public interface IEntity
{
    DateTime CreatedDateTimeUtc { get; set; }

    DateTime LastModifiedDateTimeUtc { get; set; }
}
