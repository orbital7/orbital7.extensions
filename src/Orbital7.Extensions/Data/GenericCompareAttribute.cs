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
    private string CompareToPropertyName { get; set; }

    private GenericCompareOperator OperatorName { get; set; }

    public GenericCompareAttribute(
        string compareToPropertyName,
        GenericCompareOperator operatorName)
    {
        this.CompareToPropertyName = compareToPropertyName;
        this.OperatorName = operatorName;
    }

    protected override ValidationResult? IsValid(
        object? value, 
        ValidationContext validationContext)
    {
        string operstring = OperatorName == GenericCompareOperator.GreaterThan ?
        "greater than " : OperatorName == GenericCompareOperator.GreaterThanOrEqual ?
        "greater than or equal to " :
        OperatorName == GenericCompareOperator.LessThan ? "less than " :
        OperatorName == GenericCompareOperator.LessThanOrEqual ? "less than or equal to " : "";
        var basePropertyInfo = validationContext.ObjectType.GetRuntimeProperty(CompareToPropertyName);

        var valOther = basePropertyInfo?.GetValue(validationContext.ObjectInstance, null) as IComparable;
        var valThis = value as IComparable;

        if (valThis == null || valOther == null)
        {
            return new ValidationResult("Comparison values must implement IComparable.");
        }

        bool isValid = OperatorName switch
        {
            GenericCompareOperator.GreaterThan => valThis.CompareTo(valOther) > 0,
            GenericCompareOperator.GreaterThanOrEqual => valThis.CompareTo(valOther) >= 0,
            GenericCompareOperator.LessThan => valThis.CompareTo(valOther) < 0,
            GenericCompareOperator.LessThanOrEqual => valThis.CompareTo(valOther) <= 0,
            _ => false
        };

        return isValid ? 
            ValidationResult.Success :
            new ValidationResult(
                FormatErrorMessage(validationContext.DisplayName),
                validationContext.MemberName.HasText() ?
                    [validationContext.MemberName] :
                    null);
    }
}
