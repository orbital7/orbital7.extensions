namespace Orbital7.Extensions.Apis.BetterStackApi.Telemetry;

public interface ILoggingApi
{
    Task LogEventAsync(
        string sourceToken,
        string ingestingHost,
        LogEvent logEvent,
        CancellationToken cancellationToken = default);

    Task LogEventsAsync(
        string sourceToken,
        string ingestingHost,
        IEnumerable<LogEvent> logEvents,
        CancellationToken cancellationToken = default);
}
