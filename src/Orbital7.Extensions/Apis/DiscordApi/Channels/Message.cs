namespace Orbital7.Extensions.Apis.DiscordApi.Channels;

public record Message
{
    [JsonPropertyName("id")]
    public string? Id { get; init; }

    [JsonPropertyName("channel_id")]
    public string? ChannelId { get; init; }

    [JsonPropertyName("content")]
    public string? Content { get; init; }

    [JsonPropertyName("timestamp")]
    public string? Timestamp { get; init; }
}
