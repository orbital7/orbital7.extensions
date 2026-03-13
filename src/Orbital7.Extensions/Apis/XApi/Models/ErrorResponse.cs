namespace Orbital7.Extensions.Apis.XApi;

public record ErrorResponse
{
    public string? Error { get; init; }

    public string? Message { get; init; }

    public override string ToString()
    {
        return $"{this.Message} ({this.Error})";
    }
}
