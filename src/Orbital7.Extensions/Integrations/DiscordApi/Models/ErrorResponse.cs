namespace Orbital7.Extensions.Integrations.DiscordApi;

public class ErrorResponse
{
    [JsonPropertyName("code")]
    public long Code { get; set; }

    [JsonPropertyName("message")]
    public string Message { get; set; }

    public override string ToString()
    {
        return $"{this.Message} ({this.Code})";
    }
}
