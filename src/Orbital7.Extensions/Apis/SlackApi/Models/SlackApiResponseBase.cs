namespace Orbital7.Extensions.Apis.SlackApi;

public abstract class SlackApiResponseBase
{
    [JsonPropertyName("ok")]
    public bool Ok { get; set; }

    [JsonPropertyName("error")]
    public string? Error { get; set; }

    [JsonPropertyName("warning")]
    public string? Warning { get; set; }

    public void AssertOk()
    {
        if (!this.Ok)
        {
            throw new Exception(this.Error);
        }
    }
}
