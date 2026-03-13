namespace Orbital7.Extensions.Data;

public record NamedValue<TValue>
{
    public string? Name { get; init; }

    public TValue? Value { get; init; }

    public NamedValue()
    {

    }

    public NamedValue(
        string name,
        TValue? value)
    {
        this.Name = name;
        this.Value = value;
    }

    public override string ToString()
    {
        return $"{this.Name}: {this.Value}";
    }
}
