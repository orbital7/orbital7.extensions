namespace Orbital7.Extensions.Apis.BetterStack;

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
        LogEvent logEvent)
    {
        await this.Client.SendPostRequestAsync<LogEvent, string>(
            this.BaseUrl,
            logEvent);
    }

    public async Task LogEventsAsync(
        IEnumerable<LogEvent> logEvents)
    {
        await this.Client.SendPostRequestAsync<IEnumerable<LogEvent>, string>(
            this.BaseUrl,
            logEvents);
    }
}
