namespace Orbital7.Extensions.Integrations.DiscordApi;

public interface IDiscordApiClient :
    IApiClient
{
    string BotToken { set; }
}
