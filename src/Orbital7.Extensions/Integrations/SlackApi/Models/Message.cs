namespace Orbital7.Extensions.Integrations.SlackApi;

public class Message
{
    [JsonPropertyName("user")]
    public string User { get; set; }

    [JsonPropertyName("type")]
    public string Type { get; set; }

    [JsonPropertyName("ts")]
    public string Ts { get; set; }

    [JsonPropertyName("bot_id")]
    public string BotId { get; set; }

    [JsonPropertyName("app_id")]
    public string AppId { get; set; }

    [JsonPropertyName("text")]
    public string Text { get; set; }

    [JsonPropertyName("team")]
    public string Team { get; set; }
}
