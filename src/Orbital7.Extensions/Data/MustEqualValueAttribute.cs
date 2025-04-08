namespace Orbital7.Extensions.Data;

public class MustEqualValueAttribute :
    ValidationAttribute
{
    public object Value { get; private set; }

    public MustEqualValueAttribute(
        object value)
    {
        Value = value;
    }

    public override bool IsValid(
        object? value)
    {
        return Equals(value, Value);
    }
}