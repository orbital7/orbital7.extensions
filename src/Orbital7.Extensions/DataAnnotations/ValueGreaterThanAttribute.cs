namespace System.ComponentModel.DataAnnotations;

public class ValueGreaterThanAttribute : ValueCompareAttributeBase
{
    protected override string CompareAction
    {
        get { return "greater than"; }
    }

    public ValueGreaterThanAttribute(string propertyName, bool allowEqualValues = false)
        : base(propertyName, allowEqualValues)
    {
        
    }

    protected override bool CompareValue(double thisValue, double compareValue)
    {
        return thisValue > compareValue;
    }
}
