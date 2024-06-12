namespace System.ComponentModel.DataAnnotations;

public class ObjectValidationResult
{
    public List<ValidationResult> Results { get; set; }

    public bool IsValid { get; set; }

    public List<ValidationResult> Errors =>
        this.Results != null ?
            this.Results
                .Where(x => x.ErrorMessage.HasText())
                .ToList() :
            null;

    public override string ToString()
    {
        if (this.IsValid)
        {
            return "Valid";
        }
        else
        {
            return this.Errors.ToString(", ");
        }
    }

    internal void Append(
        ObjectValidationResult validationResult)
    {
        this.IsValid = this.IsValid && validationResult.IsValid;
        this.Results.AddRange(validationResult.Results);
    }
}