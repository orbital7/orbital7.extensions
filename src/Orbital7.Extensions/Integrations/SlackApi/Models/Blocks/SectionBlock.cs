namespace Orbital7.Extensions.Integrations.SlackApi;

public class SectionBlock
{
    [JsonInclude]
    [JsonPropertyName("type")]
    public string Type { get; } = "section";

    [JsonPropertyName("text")]
    public object Text { get; set; }
}
