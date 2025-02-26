namespace Orbital7.Extensions.Integrations.BetterStackApi;

// Documentation: https://betterstack.com/docs/logs/ingesting-data/http/logs/
public class LogsUploadApi :
    LogsApiBase, ILogsUploadApi
{
    public LogsUploadApi(
        IBetterStackApiClient client) :
        base(client)
    {

    }

    public async Task LogEventAsync(
        string sourceToken,
        string ingestingHost,
        LogEvent logEvent)
    {
        this.Client.BearerToken = sourceToken;
        await this.Client.SendPostRequestAsync<LogEvent, string>(
            GetUploadUrl(ingestingHost),
            logEvent);
    }

    public async Task LogEventsAsync(
        string sourceToken,
        string ingestingHost,
        IEnumerable<LogEvent> logEvents)
    {
        this.Client.BearerToken = sourceToken;
        await this.Client.SendPostRequestAsync<IEnumerable<LogEvent>, string>(
            GetUploadUrl(ingestingHost),
            logEvents);
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
