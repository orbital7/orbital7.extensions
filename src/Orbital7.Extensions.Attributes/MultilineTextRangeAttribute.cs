using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orbital7.Extensions.Attributes
{
    public class MultilineTextRangeAttribute : ValidationAttribute
    {
        private readonly int _maxLength;
        private readonly int _maxLines;
        private readonly int _maxLineLength;

        public MultilineTextRangeAttribute(int maxLength = 0, int maxLines = 0, int maxLineLength = 0)
        {
            _maxLength = maxLength;
            _maxLines = maxLines;
            _maxLineLength = maxLineLength;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            string validationValue = value?.ToString();
            if (!string.IsNullOrEmpty(validationValue))
            {
                if (validationValue.Length > _maxLength)
                {
                    return GetErrorResult(validationContext);
                }
                else
                {
                    var lines = validationValue.ParseLines(false);
                    if (lines.Length > _maxLines)
                    {
                        return GetErrorResult(validationContext);
                    }
                    else
                    {
                        foreach (var line in lines)
                            if (line.Length > _maxLineLength)
                                return GetErrorResult(validationContext);
                    }
                }
            }

            return ValidationResult.Success;
        }

        private ValidationResult GetErrorResult(ValidationContext validationContext)
        {
            return new ValidationResult(string.Format("The {0} field has a maximum length of {1} chars total and a maximum of {2} lines total, where each line cannot exceed {3} characters",
                validationContext.MemberName, _maxLength, _maxLines, _maxLineLength));
        }
    }
}
