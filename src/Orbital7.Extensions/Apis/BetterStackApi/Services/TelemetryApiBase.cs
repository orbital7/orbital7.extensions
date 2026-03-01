namespace Orbital7.Extensions.Apis.BetterStackApi;

public abstract class TelemetryApiBase :
    ApiBase<IBetterStackApiClient>
{
    protected override string BaseUrl => "https://telemetry.betterstack.com/api/v1/";

    protected TelemetryApiBase(
        IBetterStackApiClient client) :
        base(client)
    {

    }
}
