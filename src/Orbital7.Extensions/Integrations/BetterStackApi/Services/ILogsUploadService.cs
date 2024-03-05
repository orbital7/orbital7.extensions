namespace Orbital7.Extensions.Integrations.BetterStackApi;

public interface ILogsUploadService
{
    Task LogEventAsync(
        string sourceToken,
        LogEvent logEvent);

    Task LogEventsAsync(
        string sourceToken,
        IEnumerable<LogEvent> logEvents);
}
