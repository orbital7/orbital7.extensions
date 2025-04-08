namespace Orbital7.Extensions.Integrations.SlackApi;

public class PostMessageResponse :
    SlackApiResponseBase
{
    [JsonPropertyName("channel")]
    public string? Channel { get; set; }

    [JsonPropertyName("ts")]
    public string? Ts { get; set; }

    [JsonPropertyName("message")]
    public Message? Message { get; set; }
}
