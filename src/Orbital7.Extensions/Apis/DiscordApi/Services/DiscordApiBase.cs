namespace Orbital7.Extensions.Apis.DiscordApi;

public abstract class DiscordApiBase
{
    public IDiscordApiClient Client { get; init; }

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
