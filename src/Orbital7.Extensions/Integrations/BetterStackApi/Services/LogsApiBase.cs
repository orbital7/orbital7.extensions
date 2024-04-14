namespace Orbital7.Extensions.Integrations.BetterStackApi;

public abstract class LogsApiBase :
    BetterStackApiBase
{
    public override string BaseUrl => "https://logs.betterstack.com/api/v1/";

    protected LogsApiBase(
        IBetterStackApiClient client) :
        base(client)
    {

    }
}
