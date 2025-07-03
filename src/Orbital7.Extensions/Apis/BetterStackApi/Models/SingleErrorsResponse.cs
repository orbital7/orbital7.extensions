namespace Orbital7.Extensions.Apis.BetterStackApi;

public class SingleErrorsResponse
{
    [JsonPropertyName("errors")]
    public string? Errors { get; set; }
}
