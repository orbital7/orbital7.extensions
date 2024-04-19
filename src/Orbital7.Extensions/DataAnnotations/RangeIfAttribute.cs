namespace System.ComponentModel.DataAnnotations;

public class RangeIfAttribute : 
    ConditionalValidationAttributeBase
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
