namespace Orbital7.Extensions.Apis.SlackApi;

public class PostMessageRequest
{
    [Required]
    [JsonPropertyName("channel")]
    public string? Channel { get; set; }

    [Required]
    [JsonPropertyName("text")]
    public string? Text { get; set; }

    [JsonPropertyName("blocks")]
    public List<object>? Blocks { get; set; }

    [JsonPropertyName("thread_ts")]
    public string? ThreadTs { get; set; }

    [JsonPropertyName("username")]
    public string? Username { get; set; }

    [JsonPropertyName("icon_url")]
    public string? IconUrl { get; set; }

    [JsonPropertyName("icon_emoji")]
    public string? IconEmoji { get; set; }
}
