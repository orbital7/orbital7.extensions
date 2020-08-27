using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace System.ComponentModel.DataAnnotations
{
    public abstract class ValueCompareAttribute : ValidationAttribute
    {
        protected string PropertyName { get; set; }
        protected readonly bool AllowEqualValues;

        protected abstract string CompareAction { get; }

        protected abstract bool CompareValue(double thisValue, double compareValue);

        protected ValueCompareAttribute(string propertyName, bool allowEqualValues = false)
        {
            this.PropertyName = propertyName;
            this.AllowEqualValues = allowEqualValues;
            this.ErrorMessage = "The {0} field must be " + this.CompareAction + " the {1} field";
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var compareProperty = validationContext.ObjectType.GetRuntimeProperty(this.PropertyName);
            if (compareProperty == null)
                return new ValidationResult(string.Format("unknown property {0}", this.PropertyName));

            var comparePropertyValue = Double.Parse(compareProperty.GetValue(validationContext.ObjectInstance, null).ToString());
            var thisValue = Double.Parse(value.ToString());

            if (CompareValue(thisValue, comparePropertyValue) || (this.AllowEqualValues && thisValue == comparePropertyValue))
                return ValidationResult.Success;

            return new ValidationResult(string.Format(this.ErrorMessage, validationContext.DisplayName,
                validationContext.ObjectType.GetPropertyDisplayName(this.PropertyName)));
        }
    }
}
