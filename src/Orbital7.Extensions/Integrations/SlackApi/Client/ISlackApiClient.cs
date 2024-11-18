namespace Orbital7.Extensions.Integrations.SlackApi;

public interface ISlackApiClient :
    IApiClient
{
    string BearerToken { set; }
}
