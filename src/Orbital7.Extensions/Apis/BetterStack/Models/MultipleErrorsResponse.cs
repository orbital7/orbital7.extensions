namespace Orbital7.Extensions.Apis.BetterStack;

public class MultipleErrorsResponse
{
    [JsonPropertyName("errors")]
    public IDictionary<string, string[]> Errors { get; set; }
}
