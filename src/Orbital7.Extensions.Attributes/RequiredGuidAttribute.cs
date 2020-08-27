using System;

namespace System.ComponentModel.DataAnnotations
{
    public class RequiredGuidAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            return value != null && !((Guid)value).Equals(Guid.Empty) ? ValidationResult.Success : new ValidationResult(base.ErrorMessage);
        }
    }
}
