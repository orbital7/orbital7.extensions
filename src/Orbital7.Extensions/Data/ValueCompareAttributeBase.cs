namespace Orbital7.Extensions.Data;

public abstract class ValueCompareAttributeBase : 
    ValidationAttribute
{
    protected string CompareToPropertyName { get; set; }
    protected readonly bool AllowEqualValues;

    protected abstract string CompareAction { get; }

    protected abstract bool CompareValue(
        double thisValue, 
        double compareValue);

    protected ValueCompareAttributeBase(
        string compareToPropertyName, 
        bool allowEqualValues = false)
    {
        CompareToPropertyName = compareToPropertyName;
        AllowEqualValues = allowEqualValues;
        ErrorMessage = "The {0} field must be " + CompareAction + " the {1} field";
    }

    protected override ValidationResult? IsValid(
        object? value, 
        ValidationContext validationContext)
    {
        if (value == null)
        {
            throw new ArgumentNullException(nameof(value), "Value cannot be null");
        }

        var compareProperty = validationContext.ObjectType.GetRuntimeProperty(CompareToPropertyName);
        if (compareProperty == null)
            return new ValidationResult($"Unknown property {CompareToPropertyName}");

        var comparePropertyValue = compareProperty.GetValue(validationContext.ObjectInstance, null)?.ToString();
        if (comparePropertyValue == null)
            return new ValidationResult($"Property {CompareToPropertyName} value is null");

        if (!double.TryParse(comparePropertyValue, out var compareValue))
            return new ValidationResult($"Property {CompareToPropertyName} value is not a valid number");

        if (!double.TryParse(value.ToString(), out var thisValue))
            return new ValidationResult($"Value is not a valid number");

        if (CompareValue(thisValue, compareValue) || (AllowEqualValues && thisValue == compareValue))
            return ValidationResult.Success;

        return new ValidationResult(
                string.Format(
                    ErrorMessage ?? string.Empty, 
                    validationContext.DisplayName,
                    validationContext.ObjectType.GetPropertyDisplayName(CompareToPropertyName)), 
                validationContext.MemberName.HasText() ?
                    [validationContext.MemberName] :
                    null);
    }
}
