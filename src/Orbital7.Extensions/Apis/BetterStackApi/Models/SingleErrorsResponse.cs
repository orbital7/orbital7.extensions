namespace Orbital7.Extensions.Apis.BetterStackApi;

public record SingleErrorsResponse
{
    [JsonPropertyName("errors")]
    public string? Errors { get; init; }
}
