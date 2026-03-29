namespace Orbital7.Extensions.Apis.BetterStackApi.Uptime.Heartbeats;

public record Heartbeat
{
    [JsonPropertyName("id")]
    public string? Id { get; init; }

    [JsonPropertyName("type")]
    public string? Type { get; init; }

    [JsonPropertyName("attributes")]
    public HeartbeatAttributes? Attributes { get; init; }

    public override string? ToString()
    {
        return this.Attributes?.ToString() ?? this.Id;
    }
}