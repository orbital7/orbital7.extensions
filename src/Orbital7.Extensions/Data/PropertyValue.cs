namespace Orbital7.Extensions.Data;

public record PropertyValue :
    NamedValue<object>
{
    public string? DisplayName { get; init; }
}
