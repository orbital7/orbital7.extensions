namespace Orbital7.Extensions.AspNetCore.RapidApp;

public class RAFormSubmissionResult
{
    public bool Succeeded { get; set; }

    public object ReturnedResult { get; set; }

    public RAFormValidationState ValidationState { get; set; }
}
