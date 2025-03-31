namespace Orbital7.Extensions.Integrations.BetterStackApi;

public abstract class TelemetryApiBase :
    BetterStackApiBase
{
    public override string BaseUrl => "https://telemetry.betterstack.com/api/v1/";

    protected TelemetryApiBase(
        IBetterStackApiClient client) :
        base(client)
    {

    }
}
