namespace Orbital7.Extensions.Apis.BetterStackApi;

public abstract class UptimeApiBase :
    BetterStackApiBase
{
    public override string BaseUrl => "https://uptime.betterstack.com/api/v2/";

    protected UptimeApiBase(
        IBetterStackApiClient client) : 
        base(client)
    {

    }
}
