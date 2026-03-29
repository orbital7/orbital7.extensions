namespace Orbital7.Extensions.Apis.BetterStackApi.Uptime;

public abstract class UptimeApiBase :
    ApiBase<IBetterStackApiClient>
{
    protected override string BaseUrl => "https://uptime.betterstack.com/api/v2/";

    protected UptimeApiBase(
        IBetterStackApiClient client) : 
        base(client)
    {

    }
}
