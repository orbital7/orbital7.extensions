namespace Orbital7.Extensions.Data;

public abstract class ValueCompareAttributeBase : ValidationAttribute
{
    protected string CompareToPropertyName { get; set; }
    protected readonly bool AllowEqualValues;

    protected abstract string CompareAction { get; }

    protected abstract bool CompareValue(double thisValue, double compareValue);

    protected ValueCompareAttributeBase(string compareToPropertyName, bool allowEqualValues = false)
    {
        CompareToPropertyName = compareToPropertyName;
        AllowEqualValues = allowEqualValues;
        ErrorMessage = "The {0} field must be " + CompareAction + " the {1} field";
    }

    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        var compareProperty = validationContext.ObjectType.GetRuntimeProperty(CompareToPropertyName);
        if (compareProperty == null)
            return new ValidationResult(string.Format("unknown property {0}", CompareToPropertyName));

        var comparePropertyValue = double.Parse(compareProperty.GetValue(validationContext.ObjectInstance, null).ToString());
        var thisValue = double.Parse(value.ToString());

        if (CompareValue(thisValue, comparePropertyValue) || AllowEqualValues && thisValue == comparePropertyValue)
            return ValidationResult.Success;

        return new ValidationResult(string.Format(ErrorMessage, validationContext.DisplayName,
            validationContext.ObjectType.GetPropertyDisplayName(CompareToPropertyName)), new[] { validationContext.MemberName });
    }
}
