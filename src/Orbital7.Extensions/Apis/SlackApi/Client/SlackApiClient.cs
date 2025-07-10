namespace Orbital7.Extensions.Apis.SlackApi;

public class SlackApiClient :
    ApiClient, ISlackApiClient
{
    public string? BearerToken { private get; set; }

    // Use 20s timeout.
    protected override string? HttpClientName => HttpClientFactoryHelper.HTTP_CLIENT_NAME_TIMEOUT_20S;

    public SlackApiClient(
        IHttpClientFactory httpClientFactory,
        string? bearerToken = null) :
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
