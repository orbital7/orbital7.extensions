namespace Orbital7.Extensions.Data;

public class ObjectValidationResult
{
    public List<ValidationResult> Results { get; set; } = new();

    public bool IsValid { get; set; }

    public List<ValidationResult> Errors =>
        Results != null ?
            Results
                .Where(x => x.ErrorMessage.HasText())
                .ToList() :
            new();

    public override string ToString()
    {
        if (IsValid)
        {
            return "Valid";
        }
        else
        {
            return Errors.ToString(", ");
        }
    }

    internal void Append(
        ObjectValidationResult validationResult)
    {
        IsValid = IsValid && validationResult.IsValid;
        Results.AddRange(validationResult.Results);
    }
}