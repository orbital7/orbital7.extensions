namespace Orbital7.Extensions.Data;

internal class TypeSpecificRequiredAttribute : 
    RequiredAttribute
{
    protected override ValidationResult? IsValid(
        object? value, 
        ValidationContext validationContext)
    {
        if (value is Guid)
        {
            return !((Guid)value).Equals(Guid.Empty) ?
                ValidationResult.Success :
                new ValidationResult(
                    FormatErrorMessage(validationContext.DisplayName),
                    validationContext.MemberName.HasText() ?
                        [validationContext.MemberName] :
                        null);
        }
        else
        {
            return base.IsValid(value, validationContext);
        }
    }
}
