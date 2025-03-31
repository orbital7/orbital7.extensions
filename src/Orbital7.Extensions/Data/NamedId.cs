namespace Orbital7.Extensions.Data;

public class NamedId<TId>
{
    public TId Id { get; set; }

    public string Name { get; set; }

    public NamedId()
    {

    }

    public NamedId(
        TId id,
        string name)
    {
        this.Id = id;
        this.Name = name;
    }

    public override string ToString()
    {
        return this.Name;
    }
}
