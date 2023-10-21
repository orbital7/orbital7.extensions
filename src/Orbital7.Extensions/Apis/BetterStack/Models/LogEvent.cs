namespace Orbital7.Extensions.Apis.BetterStack;

public class LogEvent
{
    [JsonPropertyName("message")]
    public string Message { get; set; }

    [JsonPropertyName("level")]
    public string Level { get; set; }

    [JsonPropertyName("metadata")]
    public IDictionary<string, object> Metadata { get; set; }

    [JsonPropertyName("dt")]
    public DateTime Timestamp { get; set; }

    public LogEvent()
    {
        this.Timestamp = DateTime.UtcNow;
    }

    public override string ToString()
    {
        return $"{this.Level}: {this.Message}";
    }
}
