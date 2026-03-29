namespace Orbital7.Extensions.Apis.SlackApi.Chat;

public record SectionBlock
{
    [JsonInclude]
    [JsonPropertyName("type")]
    public string? Type { get; } = "section";

    [JsonPropertyName("text")]
    public object? Text { get; set; }
}
