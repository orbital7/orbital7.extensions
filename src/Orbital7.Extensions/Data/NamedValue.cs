namespace Orbital7.Extensions.Data;

public class NamedValue<TValue>
{
    public required string Name { get; init; }

    public TValue? Value { get; set; }

    public override string ToString()
    {
        return $"{this.Name}: {this.Value}";
    }
}
