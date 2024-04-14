namespace Orbital7.Extensions.Integrations.BetterStackApi;

public class LogsUploadApi :
    LogsApiBase, ILogsUploadApi
{
    public override string BaseUrl => "https://in.logs.betterstack.com/";

    public LogsUploadApi(
        IBetterStackApiClient client) :
        base(client)
    {

    }

    public async Task LogEventAsync(
        string sourceToken,
        LogEvent logEvent)
    {
        this.Client.BearerToken = sourceToken;
        await this.Client.SendPostRequestAsync<LogEvent, string>(
            this.BaseUrl,
            logEvent);
    }

    public async Task LogEventsAsync(
        string sourceToken,
        IEnumerable<LogEvent> logEvents)
    {
        this.Client.BearerToken = sourceToken;
        await this.Client.SendPostRequestAsync<IEnumerable<LogEvent>, string>(
            this.BaseUrl,
            logEvents);
    }
}
