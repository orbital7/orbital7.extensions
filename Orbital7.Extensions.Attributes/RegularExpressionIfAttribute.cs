using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Orbital7.Extensions.Attributes
{
    public partial class RegularExpressionIfAttribute : ConditionalValidationAttribute
    {
        private readonly string pattern;

        public RegularExpressionIfAttribute(string pattern, string dependentProperty, object targetValue)
            : base(new RegularExpressionAttribute(pattern), dependentProperty, targetValue)
        {
            this.pattern = pattern;
        }
    }
}
