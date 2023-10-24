namespace Orbital7.Extensions.Integrations.BetterStackApi;

public class SingleErrorsResponse
{
    [JsonPropertyName("errors")]
    public string Errors { get; set; }
}
