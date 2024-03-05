namespace Orbital7.Extensions.Integrations.BetterStackApi;

public class LogsUploadService :
    LogsServiceBase, ILogsUploadService
{
    public override string BaseUrl => "https://in.logs.betterstack.com/";

    public LogsUploadService(
        IBetterStackClient client) :
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
