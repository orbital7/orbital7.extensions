using System.Runtime.CompilerServices;
using Orbital7.Extensions.Integrations.BetterStackApi;

namespace Orbital7.Extensions.Notifications;

public abstract class BetterStackLoggingServiceBase<TCategoryName> :
    ILoggingService
{
    private const string METADATA_LOGGER = "Logger";
    private const string METADATA_EXCEPTION = "Exception";
    private const string METADATA_CALLERMEMBERNAME = "CallerMemberName";
    public const string PLATFORM = "BetterStack";

    private readonly ITelemetryLoggingApi _telementryLoggingApi;
    private readonly IExternalNotificationService? _externalNotificationService;

    protected abstract string BetterStackLogsSourceToken { get; }

    protected abstract string BetterStackLogsIngestingHost { get; }

    protected BetterStackLoggingServiceBase(
        ITelemetryLoggingApi telementryLoggingApi,
        IExternalNotificationService? externalNotificationService = null)
    {
        _telementryLoggingApi = telementryLoggingApi;
        _externalNotificationService = externalNotificationService;
    }

    public virtual async Task LogAsync(
        LogLevel logLevel,
        string message,
        Exception? exception = null,
        IDictionary<string, object?>? metadata = null,
        [CallerMemberName] string? callerMemberName = null,
        bool sendExternalNotification = false)
    {
        // Validate token configuration and log level.
        if (logLevel != LogLevel.None &&
            this.BetterStackLogsSourceToken.HasText() &&
            this.BetterStackLogsIngestingHost.HasText())
        {
            var logger = typeof(TCategoryName).FullName ?? typeof(TCategoryName).Name;
            string externalNotificationMessage = string.Empty;

            try
            {
                #if DEBUG
                    Console.WriteLine($"{DateTime.Now.ToDefaultDateTimeString()} [{logLevel.ToString().ToUpper()}] {logger}: {message}");
                #endif

                // Create the metadata collection.
                var logMetadata = GetLogMetadata(
                    logLevel,
                    message,
                    exception,
                    metadata,
                    logger,
                    callerMemberName);

                // Form the external notification message.
                if (_externalNotificationService != null &&
                    sendExternalNotification)
                {
                    externalNotificationMessage = 
                        GetExternalNotificationMessage(
                            logLevel,
                            message,
                            exception,
                            logMetadata,
                            logger,
                            callerMemberName)
                        .NormalizeLineTerminators(
                            IExternalNotificationService.MSG_LINE_TERM);
                }

                // Create the log entry.
                var logEvent = new LogEvent()
                {
                    Message = message,
                    Level = GetLogLevelString(logLevel),
                    Metadata = logMetadata,
                };

                // Log the event.
                await _telementryLoggingApi.LogEventAsync(
                    this.BetterStackLogsSourceToken,
                    this.BetterStackLogsIngestingHost,
                    logEvent);
            }
            catch (Exception ex)
            {
                if (_externalNotificationService != null)
                {
                    await _externalNotificationService.SendAsync(
                        LogLevel.Error,
                        $"BetterStack Logging Error: {ex.Message.PruneEnd(".")}. " +
                            $"[{logLevel.ToString().ToUpper()}] {logger} {message}");
                }
            }

            // Send the external notification if requested.
            if (_externalNotificationService != null &&
                sendExternalNotification)
            {
                await _externalNotificationService.SendAsync(
                    logLevel,
                    externalNotificationMessage);
            }
        }
    }

    protected virtual string GetLogLevelString(
        LogLevel logLevel)
    {
        if (logLevel == LogLevel.Information)
        {
            return "Info";
        }
        else
        {
            return logLevel.ToString();
        }
    }

    protected virtual IDictionary<string, object> GetLogMetadata(
        LogLevel logLevel,
        string message,
        Exception? exception,
        IDictionary<string, object?>? metadata,
        string logger,
        string? callerMemberName)
    {
        var logMetadata = new Dictionary<string, object>
        {
            { METADATA_LOGGER, logger }
        };

        if (callerMemberName.HasText())
        {
            logMetadata.Add(METADATA_CALLERMEMBERNAME, callerMemberName);
        }

        // Add custom metadata.
        AddAdditionalLogMetadata(logMetadata);

        // Add exception metadata.
        if (exception != null)
        {
            logMetadata.Add(METADATA_EXCEPTION, new ExceptionInfo(exception));
        }

        // Add specified meta data.
        if (metadata != null && metadata.Count > 0)
        {
            foreach (var item in metadata)
            {
                // Cannot pass null dictionary item values to BetterStack.
                if (item.Key.HasText() && item.Value != null)
                {
                    logMetadata.Add(item.Key, item.Value);
                }
            }
        }

        return logMetadata;
    }

    protected virtual void AddAdditionalLogMetadata(
        IDictionary<string, object> metadata)
    {

    }

    protected virtual string GetExternalNotificationMessage(
        LogLevel logLevel,
        string message,
        Exception? exception,
        IDictionary<string, object> metadata,
        string logger,
        string? callerMemberName)
    {
        var sb = new StringBuilder();

        sb.AppendLine($"**{message}**");
        sb.AppendLine();
        sb.AppendLine($"* Source: {logger}");
        sb.AppendLine($"* Caller: {callerMemberName}()");

        var externalNotificationBulletLines = GetAdditionalExternalNotificationMessageBulletLines(
            logLevel,
            message,
            exception,
            metadata,
            logger,
            callerMemberName);

        if (externalNotificationBulletLines != null)
        {
            foreach (var bulletLine in externalNotificationBulletLines)
            {
                sb.AppendLine($"* {bulletLine}");
            }
        }

        if (exception != null)
        {
            sb.AppendLine($"* Error: {exception.Message}");
        }

        sb.AppendLine("--------------");
        sb.AppendLine();

        return sb.ToString();
    }

    protected virtual List<string>? GetAdditionalExternalNotificationMessageBulletLines(
        LogLevel logLevel,
        string message,
        Exception? exception,
        IDictionary<string, object> metadata,
        string logger,
        string? callerMemberName)
    {
        return null;
    }
}
