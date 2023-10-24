namespace Orbital7.Extensions.Integrations.BetterStackApi;

public abstract class LogsServiceBase :
    BetterStackServiceBase
{
    public override string BaseUrl => "https://logs.betterstack.com/api/v1/";

    protected LogsServiceBase(
        IBetterStackClient client) :
        base(client)
    {

    }
}
