namespace Orbital7.Extensions.Apis.BetterStackApi;

public class MultipleErrorsResponse
{
    [JsonPropertyName("errors")]
    public IDictionary<string, string[]>? Errors { get; set; }
}
