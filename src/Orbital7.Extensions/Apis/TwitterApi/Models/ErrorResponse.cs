namespace Orbital7.Extensions.Apis.TwitterApi;

public class ErrorResponse
{
    public string? Error { get; set; }

    public string? Message { get; set; }

    public override string ToString()
    {
        return $"{this.Message} ({this.Error})";
    }
}
