namespace Orbital7.Extensions.Apis.BetterStackApi;

public record MultipleErrorsResponse
{
    [JsonPropertyName("errors")]
    public IDictionary<string, string[]>? Errors { get; init; }
}
