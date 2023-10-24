namespace Orbital7.Extensions.Integrations.BetterStackApi;

public interface ILogsUploadService
{
    Task LogEventAsync(
        LogEvent logEvent);

    Task LogEventsAsync(
        IEnumerable<LogEvent> logEvents);
}
