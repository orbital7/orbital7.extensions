namespace Orbital7.Extensions.Integrations.SlackApi;

public abstract class SlackApiBase
{
    public ISlackApiClient Client { get; set; }

    protected SlackApiBase(
        ISlackApiClient client)
    {
        this.Client = client;
    }

    protected string BuildRequestUrl(
        string endpointUrl)
    {
        return $"https://slack.com/api/{endpointUrl}";
    }
}