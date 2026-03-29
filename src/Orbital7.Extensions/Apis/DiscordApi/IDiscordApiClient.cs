namespace Orbital7.Extensions.Apis.DiscordApi;

public interface IDiscordApiClient :
    IApiClient
{
    string BotToken { set; }
}
