namespace Orbital7.Extensions.Apis.SlackApi.Chat;

public record PostMessageResponse :
    SlackApiResponseBase
{
    [JsonPropertyName("channel")]
    public string? Channel { get; init; }

    [JsonPropertyName("ts")]
    public string? Ts { get; init; }

    [JsonPropertyName("message")]
    public Message? Message { get; init; }
}
