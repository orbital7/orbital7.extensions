namespace Orbital7.Extensions.Data;

public class PropertyFormatter<T>
    where T : class
{
    public Expression<Func<T, object>> Property { get; init; }

    public string? DisplayName { get; init; }

    public Action<DisplayValueOptions>? ConfigureDisplayValueOptions { get; init; }

    public Func<T, TimeConverter?, DisplayValueOptions, object?>? GetDisplayValue { get; init; }


    public PropertyFormatter(
        Expression<Func<T, object>> property,
        string? displayName = null,
        Action<DisplayValueOptions>? configureDisplayValueOptions = null,
        Func<T, TimeConverter?, DisplayValueOptions, object?>? getDisplayValue = null)
    {
        this.Property = property;
        this.DisplayName = displayName;
        this.ConfigureDisplayValueOptions = configureDisplayValueOptions;
        this.GetDisplayValue = getDisplayValue;
    }
}
