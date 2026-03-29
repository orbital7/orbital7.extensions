namespace Orbital7.Extensions.Apis.BetterStackApi.Uptime;

public record SingleErrorsResponse
{
    [JsonPropertyName("errors")]
    public string? Errors { get; init; }
}
