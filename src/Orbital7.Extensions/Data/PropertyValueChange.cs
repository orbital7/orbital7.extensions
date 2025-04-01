namespace Orbital7.Extensions.Data;

public class PropertyValueChange
{
    public required string PropertyName { get; init; }

    public required string PropertyDisplayName { get; init; }

    public string? OldValue { get; set; }

    public string? NewValue { get; set; }
}
