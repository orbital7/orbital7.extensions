using System;
using System.ComponentModel.DataAnnotations;

namespace Orbital7.Extensions.Attributes
{
    public class TypeSpecificRequiredAttribute : RequiredAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is Guid)
                return !((Guid)value).Equals(Guid.Empty) ? ValidationResult.Success : new ValidationResult(base.ErrorMessage);
            else
                return base.IsValid(value, validationContext);
        }
    }
}
