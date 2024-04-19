namespace System.ComponentModel.DataAnnotations;

public class MustEqualValueAttribute :
    ValidationAttribute
{
    public object Value { get; private set; }

    public MustEqualValueAttribute(
        object value)
    {
        this.Value = value;
    }

    public override bool IsValid(
        object value)
    {
        return Equals(value, this.Value);
    }
}