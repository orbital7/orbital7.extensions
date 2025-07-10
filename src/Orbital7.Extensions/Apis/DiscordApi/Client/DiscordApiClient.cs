
namespace Orbital7.Extensions.Apis.DiscordApi;

public class DiscordApiClient :
    ApiClient, IDiscordApiClient
{
    public string? BotToken { private get; set; }

    // Use 20s timeout.
    protected override string? HttpClientName => HttpClientFactoryHelper.HTTP_CLIENT_NAME_TIMEOUT_20S;

    public DiscordApiClient(
        IHttpClientFactory httpClientFactory,
        string? botToken = null) :
        base(httpClientFactory)
    {
        this.BotToken = botToken;
    }

    protected override void AddRequestHeaders(
        HttpRequestMessage httpRequest)
    {
        httpRequest.AddAuthorizationHeader(
            "Bot", 
            this.BotToken);
    }

    protected override Exception CreateUnsuccessfulResponseException(
        HttpResponseMessage httpResponse, 
        string responseBody)
    {
        var response = JsonSerializationHelper.DeserializeFromJson<ErrorResponse>(responseBody);
        return new Exception(response.ToString());
    }
}
