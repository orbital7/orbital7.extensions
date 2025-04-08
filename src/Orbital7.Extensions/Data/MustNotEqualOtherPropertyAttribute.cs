namespace Orbital7.Extensions.Data;

public class MustNotEqualOtherPropertyAttribute : ValidationAttribute
{
    public string OtherProperty { get; private set; }

    public MustNotEqualOtherPropertyAttribute(
        string otherProperty)
    {
        OtherProperty = otherProperty;
    }

    protected override ValidationResult? IsValid(
        object? value, 
        ValidationContext validationContext)
    {
        var property = validationContext.ObjectType.GetRuntimeProperty(OtherProperty);
        if (property == null)
        {
            return new ValidationResult(
                string.Format(
                    CultureInfo.CurrentCulture,
                    "{0} is unknown property",
                    OtherProperty
                )
            );
        }
        var otherValue = property.GetValue(validationContext.ObjectInstance, null);
        if (Equals(value, otherValue))
        {
            return new ValidationResult(
                FormatErrorMessage(validationContext.DisplayName), 
                validationContext.MemberName.HasText() ? 
                    [validationContext.MemberName] : 
                    null);
        }
        return null;
    }
}
