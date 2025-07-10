namespace Orbital7.Extensions.Apis;

public class ProblemDetailsException :
    Exception
{
    public ProblemDetailsResponse ProblemDetails { get; init; }

    public ProblemDetailsException(
        ProblemDetailsResponse problemDetails) :
        base(problemDetails.Detail ?? problemDetails.Title)
    {
        this.ProblemDetails = problemDetails;
    }
}
