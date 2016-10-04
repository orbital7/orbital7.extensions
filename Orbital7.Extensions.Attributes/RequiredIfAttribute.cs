using System.Collections.Generic;

namespace Orbital7.Extensions.Attributes
{
    public partial class RequiredIfAttribute : ConditionalValidationAttribute
    {
        public RequiredIfAttribute(string dependentProperty, object targetValue)
            : base(new TypeSpecificRequiredAttribute(), dependentProperty, targetValue)
        {
            
        }
    }
}
