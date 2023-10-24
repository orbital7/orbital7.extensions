namespace Orbital7.Extensions.Integrations.BetterStackApi;

public abstract class BetterStackServiceBase
{
    public IBetterStackClient Client { get; set; }

    public abstract string BaseUrl { get; }

    protected BetterStackServiceBase(
        IBetterStackClient client)
    {
        this.Client = client;
    }

    protected string BuildRequestUrl(
        string endpointUrl)
    {
        return $"{this.BaseUrl.PruneEnd("/")}/{endpointUrl.PruneStart("/")}";
    }
}
