namespace Orbital7.Extensions.Apis.BetterStackApi.Telemetry;

// Documentation: https://betterstack.com/docs/logs/ingesting-data/http/logs/
public class LoggingApi :
    ApiBase<IBetterStackApiClient>, ILoggingApi
{
    protected override string BaseUrl => "https://telemetry.betterstack.com/api/v1/";

    public LoggingApi(
        IBetterStackApiClient client) :
        base(client)
    {

    }

    public async Task LogEventAsync(
        string sourceToken,
        string ingestingHost,
        LogEvent logEvent,
        CancellationToken cancellationToken = default)
    {
        this.Client.BearerToken = sourceToken;
        await this.Client.SendPostRequestAsync<LogEvent, string>(
            GetUploadUrl(ingestingHost),
            logEvent,
            cancellationToken);
    }

    public async Task LogEventsAsync(
        string sourceToken,
        string ingestingHost,
        IEnumerable<LogEvent> logEvents,
        CancellationToken cancellationToken = default)
    {
        this.Client.BearerToken = sourceToken;
        await this.Client.SendPostRequestAsync<IEnumerable<LogEvent>, string>(
            GetUploadUrl(ingestingHost),
            logEvents,
            cancellationToken);
    }

    private string GetUploadUrl(
        string ingestingHost)
    {
        return "https://" +
            ingestingHost
                .ToLower()
                .PruneStart("https://");
    }
}
