using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Orbital7.Extensions.Attributes
{
    public partial class RangeIfAttribute : ConditionalValidationAttribute
    {
        private readonly int minimum;
        private readonly int maximum;
        
        public RangeIfAttribute(int minimum, int maximum, string dependentProperty, object targetValue)
            : base(new RangeAttribute(minimum, maximum), dependentProperty, targetValue)
        {
            this.minimum = minimum;
            this.maximum = maximum;
        }
    }
}
