namespace System.ComponentModel.DataAnnotations;

internal class TypeSpecificRequiredAttribute : 
    RequiredAttribute
{
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        if (value is Guid)
            return !((Guid)value).Equals(Guid.Empty) ? ValidationResult.Success : new ValidationResult(base.ErrorMessage, new[] { validationContext.MemberName });
        else
            return base.IsValid(value, validationContext);
    }
}
