namespace Orbital7.Extensions.Integrations.BetterStackApi;

public class MultipleErrorsResponse
{
    [JsonPropertyName("errors")]
    public IDictionary<string, string[]>? Errors { get; set; }
}
