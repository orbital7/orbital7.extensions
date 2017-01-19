using System.Collections;
using System.ComponentModel.DataAnnotations;

namespace Orbital7.Extensions.Attributes
{
    public class EnsureMinimumElementsAttribute : ValidationAttribute
    {
        private readonly int _minElements;

        public EnsureMinimumElementsAttribute(int minElements)
        {
            _minElements = minElements;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            return (((IList)value).Count >= _minElements) ? ValidationResult.Success : new ValidationResult(base.ErrorMessage);
        }
    }
}
