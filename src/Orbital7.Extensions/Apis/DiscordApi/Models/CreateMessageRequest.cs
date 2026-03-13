namespace Orbital7.Extensions.Apis.DiscordApi;

public record CreateMessageRequest
{
    [JsonPropertyName("content")]
    public string? Content { get; set; }
}
