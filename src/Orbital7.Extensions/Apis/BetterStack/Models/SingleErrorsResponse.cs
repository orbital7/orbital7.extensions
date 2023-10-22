namespace Orbital7.Extensions.Apis.BetterStack;

public class SingleErrorsResponse
{
    [JsonPropertyName("errors")]
    public string Errors { get; set; }
}
