namespace Orbital7.Extensions.Integrations.SlackApi;

public class MarkdownBlock
{
    [JsonInclude]
    [JsonPropertyName("type")]
    public string? Type { get; } = "mrkdwn";

    [JsonPropertyName("text")]
    public object? Text { get; set; }
}
