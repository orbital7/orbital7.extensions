namespace Orbital7.Extensions.Data;

public enum GenericCompareOperator
{
    GreaterThan,
    GreaterThanOrEqual,
    LessThan,
    LessThanOrEqual
}

public sealed class GenericCompareAttribute : 
    ValidationAttribute
{
    private GenericCompareOperator operatorname = GenericCompareOperator.GreaterThanOrEqual;

    public string CompareToPropertyName { get; set; }

    public GenericCompareOperator OperatorName 
    { 
        get 
        { 
            return operatorname; 
        } 
        set 
        { 
            operatorname = value; 
        } 
    }

    public GenericCompareAttribute() : base() { }

    protected override ValidationResult IsValid(
        object value, 
        ValidationContext validationContext)
    {
        string operstring = OperatorName == GenericCompareOperator.GreaterThan ?
        "greater than " : OperatorName == GenericCompareOperator.GreaterThanOrEqual ?
        "greater than or equal to " :
        OperatorName == GenericCompareOperator.LessThan ? "less than " :
        OperatorName == GenericCompareOperator.LessThanOrEqual ? "less than or equal to " : "";
        var basePropertyInfo = validationContext.ObjectType.GetRuntimeProperty(CompareToPropertyName);

        var valOther = (IComparable)basePropertyInfo.GetValue(validationContext.ObjectInstance, null);

        var valThis = (IComparable)value;

        if (operatorname == GenericCompareOperator.GreaterThan && valThis.CompareTo(valOther) <= 0 ||
            operatorname == GenericCompareOperator.GreaterThanOrEqual && valThis.CompareTo(valOther) < 0 ||
            operatorname == GenericCompareOperator.LessThan && valThis.CompareTo(valOther) >= 0 ||
            operatorname == GenericCompareOperator.LessThanOrEqual && valThis.CompareTo(valOther) > 0)
            return new ValidationResult(ErrorMessage, new[] { validationContext.MemberName });
        return null;
    }
}
