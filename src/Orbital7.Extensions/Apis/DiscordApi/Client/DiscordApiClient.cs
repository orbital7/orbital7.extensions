
namespace Orbital7.Extensions.Apis.DiscordApi;

public class DiscordApiClient :
    ApiClient, IDiscordApiClient
{
    public string? BotToken { private get; set; }

    public DiscordApiClient(
        IHttpClientFactory httpClientFactory,
        string? botToken = null,
        string? httpClientName = null) :
        base(httpClientFactory, httpClientName)
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
