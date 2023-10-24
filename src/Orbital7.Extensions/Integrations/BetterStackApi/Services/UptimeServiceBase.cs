namespace Orbital7.Extensions.Integrations.BetterStackApi;

public abstract class UptimeServiceBase :
    BetterStackServiceBase
{
    public override string BaseUrl => "https://uptime.betterstack.com/api/v2/";

    protected UptimeServiceBase(
        IBetterStackClient client) : 
        base(client)
    {

    }
}
