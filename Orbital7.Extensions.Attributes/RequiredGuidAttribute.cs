using System;
using System.ComponentModel.DataAnnotations;

namespace Orbital7.Extensions.Attributes
{
    public class RequiredGuidAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            return value != null && !((Guid)value).Equals(Guid.Empty) ? ValidationResult.Success : new ValidationResult(base.ErrorMessage);
        }
    }
}
