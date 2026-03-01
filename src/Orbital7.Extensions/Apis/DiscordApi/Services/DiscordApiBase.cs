namespace Orbital7.Extensions.Apis.DiscordApi;

public abstract class DiscordApiBase :
    ApiBase<IDiscordApiClient>
{
    protected override string BaseUrl => "https://discord.com/api/v9/";

    protected DiscordApiBase(
        IDiscordApiClient client) :
        base(client)
    {
        
    }
}
