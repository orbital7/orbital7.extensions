namespace Orbital7.Extensions.Apis.BetterStack;

public interface ILogsUploadService
{
    Task LogEventAsync(
        LogEvent logEvent);

    Task LogEventsAsync(
        IEnumerable<LogEvent> logEvents);
}
