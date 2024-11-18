namespace Orbital7.Extensions.Integrations.SlackApi;

public class SlackApiClient :
    ApiClient, ISlackApiClient
{
    public string BearerToken { private get; set; }

    public SlackApiClient(
        IHttpClientFactory httpClientFactory,
        string bearerToken = null) :
        base(httpClientFactory)
    {
        this.BearerToken = bearerToken;
    }

    protected override void AddRequestHeaders(
        HttpRequestMessage httpRequest)
    {
        httpRequest.AddBearerTokenAuthorizationHeader(this.BearerToken);
    }
}
