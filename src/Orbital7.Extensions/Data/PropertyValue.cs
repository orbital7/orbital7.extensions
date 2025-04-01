namespace Orbital7.Extensions.Data;

public class PropertyValue :
    NamedValue<object>
{
    public required string DisplayName { get; set; }
}
