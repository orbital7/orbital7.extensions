namespace Orbital7.Extensions.Apis.BetterStackApi.Telemetry;

public record LogEvent
{
    [JsonPropertyName("message")]
    public string? Message { get; init; }

    [JsonPropertyName("level")]
    public string? Level { get; init; }

    [JsonPropertyName("metadata")]
    public IDictionary<string, object>? Metadata { get; init; }

    [JsonPropertyName("dt")]
    public DateTime Timestamp { get; init; } = DateTime.UtcNow;

    public override string ToString()
    {
        return $"{this.Level}: {this.Message}";
    }
}
