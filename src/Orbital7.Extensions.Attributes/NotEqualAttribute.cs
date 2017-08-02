using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Reflection;

namespace Orbital7.Extensions.Attributes
{
    public partial class NotEqualAttribute : ValidationAttribute//, IClientModelValidator
    {
        public string OtherProperty { get; private set; }

        public NotEqualAttribute(string otherProperty)
        {
            OtherProperty = otherProperty;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
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
            if (object.Equals(value, otherValue))
            {
                return new ValidationResult(FormatErrorMessage(validationContext.DisplayName));
            }
            return null;
        }

        //public void AddValidation(ClientModelValidationContext context)
        //{
        //    throw new NotImplementedException();
        //}

        //public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        //{
        //    var rule = new ModelClientValidationRule
        //    {
        //        ErrorMessage = ErrorMessage,
        //        ValidationType = "notequalto",
        //    };
        //    rule.ValidationParameters["other"] = OtherProperty;
        //    yield return rule;
        //}
    }
}
