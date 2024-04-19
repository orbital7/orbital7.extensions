using System.Reflection;

namespace System.ComponentModel.DataAnnotations;

// SOURCE: http://forums.asp.net/t/1924941.aspx?Conditional+Validation+using+DataAnnotation

[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false)]
public abstract class ConditionalValidationAttributeBase : ValidationAttribute
{
    protected readonly ValidationAttribute InnerAttribute;
    public string DependentProperty { get; set; }
    public object TargetValue { get; set; }
    protected bool ShouldMatch { get; set; }

    protected ConditionalValidationAttributeBase(
        ValidationAttribute innerAttribute, 
        string dependentProperty, 
        object targetValue)
    {
        this.InnerAttribute = innerAttribute;
        this.DependentProperty = dependentProperty;
        this.TargetValue = targetValue;
        this.ErrorMessage = "The {0} field is required.";
        this.ShouldMatch = true;
    }

    public override bool IsValid(object value)
    {
        return base.IsValid(value);
    }

    protected override ValidationResult IsValid(
        object value, 
        ValidationContext validationContext)
    {
        // get a reference to the property this validation depends upon
        var containerType = validationContext.ObjectInstance.GetType();
        var field = containerType.GetRuntimeProperty(this.DependentProperty);
        if (field != null)
        {
            // get the value of the dependent property
            var dependentvalue = field.GetValue(validationContext.ObjectInstance, null);

            // compare the value against the target value
            if ((dependentvalue == null && this.TargetValue == null) || (dependentvalue != null && (dependentvalue.Equals(this.TargetValue) == this.ShouldMatch)))
            {
                // match => means we should try validating this field.
                var innerAttributeValidation = this.InnerAttribute.GetValidationResult(value, validationContext);
                if (innerAttributeValidation != null && innerAttributeValidation != ValidationResult.Success)
                    return new ValidationResult(string.Format(this.ErrorMessage, validationContext.DisplayName), new[] { validationContext.MemberName });
            }
        }
        return ValidationResult.Success;
    }
}
