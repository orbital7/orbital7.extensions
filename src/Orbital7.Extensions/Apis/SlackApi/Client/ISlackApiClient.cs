namespace Orbital7.Extensions.Apis.SlackApi;

public interface ISlackApiClient :
    IApiClient
{
    string BearerToken { set; }
}
