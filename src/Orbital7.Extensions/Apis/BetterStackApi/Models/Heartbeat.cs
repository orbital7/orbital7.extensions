namespace Orbital7.Extensions.Apis.BetterStackApi;

public class Heartbeat
{
    [JsonPropertyName("id")]
    public string? Id { get; set; }

    [JsonPropertyName("type")]
    public string? Type { get; set; }

    [JsonPropertyName("attributes")]
    public HeartbeatAttributes? Attributes { get; set; }

    public override string? ToString()
    {
        return this.Attributes?.ToString() ?? this.Id;
    }
}