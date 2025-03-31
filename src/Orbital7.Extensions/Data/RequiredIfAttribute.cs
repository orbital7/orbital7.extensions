namespace Orbital7.Extensions.Data;

public partial class RequiredIfAttribute : 
    ConditionalValidationAttributeBase
{
    public RequiredIfAttribute(string dependentProperty, object targetValue)
        : base(new TypeSpecificRequiredAttribute(), dependentProperty, targetValue)
    {
        
    }
}
