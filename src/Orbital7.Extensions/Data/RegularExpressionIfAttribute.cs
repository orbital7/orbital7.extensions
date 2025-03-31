namespace Orbital7.Extensions.Data;

public class RegularExpressionIfAttribute : 
    ConditionalValidationAttributeBase
{
    private readonly string pattern;

    public RegularExpressionIfAttribute(string pattern, string dependentProperty, object targetValue)
        : base(new RegularExpressionAttribute(pattern), dependentProperty, targetValue)
    {
        this.pattern = pattern;
    }
}
