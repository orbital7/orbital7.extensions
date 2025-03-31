namespace Orbital7.Extensions.Data;

internal class TypeSpecificRequiredAttribute : 
    RequiredAttribute
{
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        if (value is Guid)
            return !((Guid)value).Equals(Guid.Empty) ? ValidationResult.Success : new ValidationResult(ErrorMessage, new[] { validationContext.MemberName });
        else
            return base.IsValid(value, validationContext);
    }
}
