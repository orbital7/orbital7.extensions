﻿using System.Reflection;

namespace System.ComponentModel.DataAnnotations;

// SOURCE: http://forums.asp.net/t/1924941.aspx?Conditional+Validation+using+DataAnnotation

[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false)]
public abstract partial class ConditionalValidationAttribute : ValidationAttribute//, IClientModelValidator
{
    protected readonly ValidationAttribute InnerAttribute;
    public string DependentProperty { get; set; }
    public object TargetValue { get; set; }
    protected bool ShouldMatch { get; set; }

    protected ConditionalValidationAttribute(ValidationAttribute innerAttribute, string dependentProperty, object targetValue)
    {
        this.InnerAttribute = innerAttribute;
        this.DependentProperty = dependentProperty;
        this.TargetValue = targetValue;
        this.ErrorMessage = "The {0} field is required.";
        this.ShouldMatch = true;
    }

    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
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

    //protected abstract string ValidationName { get; }

    //protected virtual IDictionary<string, object> GetExtraValidationParameters()
    //{
    //    return new Dictionary<string, object>();
    //}

    //public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
    //{
    //    var rule = new ModelClientValidationRule()
    //    {
    //        ErrorMessage = FormatErrorMessage(metadata.GetDisplayName()),
    //        ValidationType = ValidationName,
    //    };
    //    string depProp = BuildDependentPropertyId(metadata, context as ViewContext);
    //    // find the value on the control we depend on; if it's a bool, format it javascript style
    //    string targetValue = (this.TargetValue ?? "").ToString();
    //    if (this.TargetValue.GetType() == typeof(bool))
    //    {
    //        targetValue = targetValue.ToLower();
    //    }
    //    rule.ValidationParameters.Add("dependentproperty", depProp);
    //    rule.ValidationParameters.Add("targetvalue", targetValue);
    //    // Add the extra params, if any
    //    foreach (var param in GetExtraValidationParameters())
    //    {
    //        rule.ValidationParameters.Add(param);
    //    }
    //    yield return rule;
    //}

    //private string BuildDependentPropertyId(ModelMetadata metadata, ViewContext viewContext)
    //{
    //    string depProp = viewContext.ViewData.TemplateInfo.GetFullHtmlFieldId(this.DependentProperty);
    //    // This will have the name of the current field appended to the beginning, because the TemplateInfo's context has had this fieldname appended to it.
    //    var thisField = metadata.PropertyName + "_";
    //    if (depProp.StartsWith(thisField))
    //    {
    //        depProp = depProp.Substring(thisField.Length);
    //    }
    //    return depProp;
    //}
}
