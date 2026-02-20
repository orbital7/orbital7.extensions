namespace Orbital7.Extensions.Apis.SlackApi;

public abstract class SlackApiBase
{
    public ISlackApiClient Client { get; init; }

    protected SlackApiBase(
        ISlackApiClient client)
    {
        this.Client = client;
    }

    protected string BuildRequestUrl(
        string endpointUrl)
    {
        return $"https://slack.com/api/{endpointUrl.PruneStart("/")}";
    }
}