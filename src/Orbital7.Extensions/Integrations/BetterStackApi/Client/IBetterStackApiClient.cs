namespace Orbital7.Extensions.Integrations.BetterStackApi;

public interface IBetterStackApiClient :
    IApiClient
{
    string BearerToken { set; }
}
