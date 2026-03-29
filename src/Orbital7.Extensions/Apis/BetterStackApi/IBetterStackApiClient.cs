namespace Orbital7.Extensions.Apis.BetterStackApi;

public interface IBetterStackApiClient :
    IApiClient
{
    string BearerToken { set; }
}
