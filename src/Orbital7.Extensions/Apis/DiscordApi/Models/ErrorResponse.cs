namespace Orbital7.Extensions.Apis.DiscordApi;

public record ErrorResponse
{
    [JsonPropertyName("code")]
    public long? Code { get; init; }

    [JsonPropertyName("message")]
    public string? Message { get; init; }

    public override string ToString()
    {
        return $"{this.Message} ({this.Code})";
    }
}
