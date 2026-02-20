namespace Orbital7.Extensions.Apis.BetterStackApi;

public abstract class BetterStackApiBase
{
    public IBetterStackApiClient Client { get; init; }

    public abstract string BaseUrl { get; }

    protected BetterStackApiBase(
        IBetterStackApiClient client)
    {
        this.Client = client;
    }

    protected string BuildRequestUrl(
        string endpointUrl)
    {
        return $"{this.BaseUrl.PruneEnd("/")}/{endpointUrl.PruneStart("/")}";
    }
}
