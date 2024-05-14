namespace System;

public class PropertyValue :
    NamedValue<object>
{
    public string DisplayName { get; set; }

    public PropertyValue()
    {

    }

    public PropertyValue(
        string name,
        string displayName,
        object value) :
        base(name, value)
    {
        this.DisplayName = displayName;
    }
}
