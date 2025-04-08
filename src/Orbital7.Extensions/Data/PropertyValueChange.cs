namespace Orbital7.Extensions.Data;

public class PropertyValueChange
{
    public string? PropertyName { get; init; }

    public string? PropertyDisplayName { get; init; }

    public string? OldValue { get; set; }

    public string? NewValue { get; set; }
}
