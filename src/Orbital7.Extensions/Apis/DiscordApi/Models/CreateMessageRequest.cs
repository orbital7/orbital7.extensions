namespace Orbital7.Extensions.Apis.DiscordApi;

public class CreateMessageRequest
{
    [JsonPropertyName("content")]
    public string? Content { get; set; }
}
