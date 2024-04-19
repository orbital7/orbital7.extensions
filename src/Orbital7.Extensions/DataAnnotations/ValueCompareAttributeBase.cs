namespace System.ComponentModel.DataAnnotations;

public abstract class ValueCompareAttributeBase : ValidationAttribute
{
    protected string CompareToPropertyName { get; set; }
    protected readonly bool AllowEqualValues;

    protected abstract string CompareAction { get; }

    protected abstract bool CompareValue(double thisValue, double compareValue);

    protected ValueCompareAttributeBase(string compareToPropertyName, bool allowEqualValues = false)
    {
        this.CompareToPropertyName = compareToPropertyName;
        this.AllowEqualValues = allowEqualValues;
        this.ErrorMessage = "The {0} field must be " + this.CompareAction + " the {1} field";
    }

    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        var compareProperty = validationContext.ObjectType.GetRuntimeProperty(this.CompareToPropertyName);
        if (compareProperty == null)
            return new ValidationResult(string.Format("unknown property {0}", this.CompareToPropertyName));

        var comparePropertyValue = Double.Parse(compareProperty.GetValue(validationContext.ObjectInstance, null).ToString());
        var thisValue = Double.Parse(value.ToString());

        if (CompareValue(thisValue, comparePropertyValue) || (this.AllowEqualValues && thisValue == comparePropertyValue))
            return ValidationResult.Success;

        return new ValidationResult(string.Format(this.ErrorMessage, validationContext.DisplayName,
            validationContext.ObjectType.GetPropertyDisplayName(this.CompareToPropertyName)), new[] { validationContext.MemberName });
    }
}
