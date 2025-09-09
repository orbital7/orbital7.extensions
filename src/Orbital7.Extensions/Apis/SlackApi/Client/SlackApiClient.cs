namespace Orbital7.Extensions.Apis.SlackApi;

public class SlackApiClient :
    ApiClient, ISlackApiClient
{
    public string? BearerToken { private get; set; }

    public SlackApiClient(
        IHttpClientFactory httpClientFactory,
        string? bearerToken = null,
        string? httpClientName = null) :
        base(httpClientFactory, httpClientName)
    {
        this.BearerToken = bearerToken;
    }

    protected override void AddRequestHeaders(
        HttpRequestMessage httpRequest)
    {
        httpRequest.AddBearerTokenAuthorizationHeader(this.BearerToken);
    }
}
