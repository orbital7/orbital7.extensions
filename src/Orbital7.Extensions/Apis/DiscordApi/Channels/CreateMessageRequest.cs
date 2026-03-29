namespace Orbital7.Extensions.Apis.DiscordApi.Channels;

public record CreateMessageRequest
{
    [JsonPropertyName("content")]
    public string? Content { get; set; }
}
