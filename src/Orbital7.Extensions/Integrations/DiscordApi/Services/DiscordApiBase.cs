namespace Orbital7.Extensions.Integrations.DiscordApi;

public abstract class DiscordApiBase
{
    public IDiscordApiClient Client { get; set; }

    protected DiscordApiBase(
        IDiscordApiClient client)
    {
        this.Client = client;
    }

    protected string BuildRequestUrl(
        string endpointUrl)
    {
        return $"https://discord.com/api/v9/{endpointUrl.PruneStart("/")}";
    }
}
