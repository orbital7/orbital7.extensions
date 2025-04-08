namespace Orbital7.Extensions.Integrations.DiscordApi;

public class Message
{
    [JsonPropertyName("id")]
    public string? Id { get; set; }

    [JsonPropertyName("channel_id")]
    public string? ChannelId { get; set; }

    [JsonPropertyName("content")]
    public string? Content { get; set; }

    [JsonPropertyName("timestamp")]
    public string? Timestamp { get; set; }
}
