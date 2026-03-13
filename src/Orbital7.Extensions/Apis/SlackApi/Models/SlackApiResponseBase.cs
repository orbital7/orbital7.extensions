namespace Orbital7.Extensions.Apis.SlackApi;

public abstract record SlackApiResponseBase
{
    [JsonPropertyName("ok")]
    public bool Ok { get; init; }

    [JsonPropertyName("error")]
    public string? Error { get; init; }

    [JsonPropertyName("warning")]
    public string? Warning { get; init; }

    public void AssertOk()
    {
        if (!this.Ok)
        {
            throw new Exception(this.Error);
        }
    }
}
