namespace Orbital7.Extensions.Integrations.DiscordApi;

public class CreateMessageRequest
{
    [JsonPropertyName("content")]
    public string Content { get; set; }
}
