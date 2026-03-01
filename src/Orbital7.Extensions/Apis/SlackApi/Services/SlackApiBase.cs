namespace Orbital7.Extensions.Apis.SlackApi;

public abstract class SlackApiBase :
    ApiBase<ISlackApiClient>
{
    protected override string BaseUrl => "https://slack.com/api/";

    protected SlackApiBase(
        ISlackApiClient client) :
        base(client)
    {
        
    }
}