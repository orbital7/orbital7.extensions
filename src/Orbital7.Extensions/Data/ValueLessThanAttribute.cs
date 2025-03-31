namespace Orbital7.Extensions.Data;

public class ValueLessThanAttribute : ValueCompareAttributeBase
{
    protected override string CompareAction
    {
        get { return "less than"; }
    }

    public ValueLessThanAttribute(string propertyName, bool allowEqualValues = false)
        : base(propertyName, allowEqualValues)
    {

    }

    protected override bool CompareValue(double thisValue, double compareValue)
    {
        return thisValue < compareValue;
    }
}
