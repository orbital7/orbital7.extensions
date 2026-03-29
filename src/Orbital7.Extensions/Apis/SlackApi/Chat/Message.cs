namespace Orbital7.Extensions.Apis.SlackApi.Chat;

public record Message
{
    [JsonPropertyName("user")]
    public string? User { get; init; }

    [JsonPropertyName("type")]
    public string? Type { get; init; }

    [JsonPropertyName("ts")]
    public string? Ts { get; init; }

    [JsonPropertyName("bot_id")]
    public string? BotId { get; init; }

    [JsonPropertyName("app_id")]
    public string? AppId { get; init; }

    [JsonPropertyName("text")]
    public string? Text { get; init; }

    [JsonPropertyName("team")]
    public string? Team { get; init; }
}
