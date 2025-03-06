using Microsoft.Extensions.Logging;
using System.Runtime.CompilerServices;
using Orbital7.Extensions.Integrations.BetterStackApi;

namespace Orbital7.Extensions.Notifications;

public abstract class BetterStackLoggingServiceBase<TCategoryName> :
    ILoggingService
{
    private const string METADATA_LOGGER = "Logger";
    private const string METADATA_EXCEPTION = "Exception";
    private const string METADATA_CALLERMEMBERNAME = "CallerMemberName";

    private readonly ILogsUploadApi _logsUploadApi;
    private readonly IExternalNotificationService _externalNotificationService;

    protected abstract string BetterStackLogsSourceToken { get; }

    protected abstract string BetterStackLogsIngestingHost { get; }

    protected BetterStackLoggingServiceBase(
        ILogsUploadApi logsUploadApi,
        IExternalNotificationService externalNotificationService = null)
    {
        _logsUploadApi = logsUploadApi;
        _externalNotificationService = externalNotificationService;
    }

    public virtual async Task LogTraceAsync(
        string message,
        IDictionary<string, object> metadata = null,
        [CallerMemberName] string callerMemberName = null,
        bool sendExternalNotification = false)
    {
        await LogAsync(
            LogLevel.Trace,
            message,
            null,
            metadata,
            callerMemberName,
            sendExternalNotification);
    }

    public virtual async Task LogDebugAsync(
        string message,
        IDictionary<string, object> metadata = null,
        [CallerMemberName] string callerMemberName = null,
        bool sendExternalNotification = false)
    {
        await LogAsync(
            LogLevel.Debug,
            message,
            null,
            metadata,
            callerMemberName,
            sendExternalNotification);
    }

    public virtual async Task LogInformationAsync(
        string message,
        IDictionary<string, object> metadata = null,
        [CallerMemberName] string callerMemberName = null,
        bool sendExternalNotification = false)
    {
        await LogAsync(
            LogLevel.Information,
            message,
            null,
            metadata,
            callerMemberName,
            sendExternalNotification);
    }

    public virtual async Task LogWarningAsync(
        string message,
        Exception exception = null,
        IDictionary<string, object> metadata = null,
        [CallerMemberName] string callerMemberName = null,
        bool sendExternalNotification = false)
    {
        await LogAsync(
            LogLevel.Warning,
            message,
            exception,
            metadata,
            callerMemberName,
            sendExternalNotification);
    }

    public virtual async Task LogErrorAsync(
        string message,
        Exception exception = null,
        IDictionary<string, object> metadata = null,
        [CallerMemberName] string callerMemberName = null,
        bool sendExternalNotification = false)
    {
        await LogAsync(
            LogLevel.Error,
            message,
            exception,
            metadata,
            callerMemberName,
            sendExternalNotification);
    }

    public virtual async Task LogCriticalAsync(
        string message,
        Exception exception = null,
        IDictionary<string, object> metadata = null,
        [CallerMemberName] string callerMemberName = null,
        bool sendExternalNotification = false)
    {
        await LogAsync(
            LogLevel.Critical,
            message,
            exception,
            metadata,
            callerMemberName,
            sendExternalNotification);
    }

    protected virtual void AddLogMetadata(
        IDictionary<string, object> metadata)
    {

    }

    protected virtual void AddExternalNotificationText(
        StringBuilder externalNotificationMessageBuilder)
    {

    }

    protected virtual async Task LogAsync(
        LogLevel logLevel, 
        string message, 
        Exception exception, 
        IDictionary<string, object> metadata, 
        string callerMemberName,
        bool sendExternalNotification)
    {
        // Validate token configuration and log level.
        if (this.BetterStackLogsSourceToken.HasText() && 
            this.BetterStackLogsIngestingHost.HasText() &&
            logLevel != LogLevel.None)
        {
            var externalNotificationMessageBuilder = new StringBuilder();
            var logger = typeof(TCategoryName).FullName;

            try
            {
                // Create the metadata collection.
                var logMetadata = new Dictionary<string, object>
                {
                    { METADATA_LOGGER, logger }
                };
                if (callerMemberName.HasText())
                {
                    logMetadata.Add(METADATA_CALLERMEMBERNAME, callerMemberName);
                }

                // Add custom metadata.
                AddLogMetadata(logMetadata);

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
                        // Apparently can't pass null dictionary item values to BetterStack.
                        if (item.Key.HasText() && item.Value != null)
                        {
                            logMetadata.Add(item.Key, item.Value);
                        }
                    }
                }

                #if DEBUG
                    Console.WriteLine($"{DateTime.Now.ToDefaultDateTimeString()} [{logLevel.ToString().ToUpper()}] {logger}: {message}");
                #endif

                // Form the external notification message.
                if (_externalNotificationService != null &&
                    sendExternalNotification)
                {
                    externalNotificationMessageBuilder.AppendLine(message);
                    externalNotificationMessageBuilder.AppendLine();
                    externalNotificationMessageBuilder.AppendLine($" * Source: {logger}");
                    externalNotificationMessageBuilder.AppendLine($" * Caller: {callerMemberName}");

                    AddExternalNotificationText(externalNotificationMessageBuilder);
                    
                    if (exception != null)
                    {
                        externalNotificationMessageBuilder.AppendLine($" * Error: {exception.Message}");
                    }

                    externalNotificationMessageBuilder.AppendLine("--------------");
                }

                // Create the log entry.
                var logEvent = new LogEvent()
                {
                    Message = message,
                    Level = logLevel.ToString(),
                    Metadata = logMetadata,
                };

                // Upload.
                await _logsUploadApi.LogEventAsync(
                    this.BetterStackLogsSourceToken,
                    this.BetterStackLogsIngestingHost,
                    logEvent);
            }
            catch (Exception ex)
            {
                await _externalNotificationService.SendErrorAsync(
                    $"BetterStackLoggingService Error: {ex.Message?.PruneEnd(".")}. " +
                    $"[{logLevel.ToString().ToUpper()}] {logger} {message}");
            }

            // Send the external notification if requested.
            if (_externalNotificationService != null &&
                sendExternalNotification)
            {
                var externalNotificationMessage = externalNotificationMessageBuilder.ToString();

                switch (logLevel)
                {
                    case LogLevel.Trace:
                        await _externalNotificationService.SendTraceAsync(externalNotificationMessage);
                        break;

                    case LogLevel.Debug:
                        await _externalNotificationService.SendDebugAsync(externalNotificationMessage);
                        break;

                    case LogLevel.Information:
                        await _externalNotificationService.SendInformationAsync(externalNotificationMessage);
                        break;

                    case LogLevel.Warning:
                        await _externalNotificationService.SendWarningAsync(externalNotificationMessage);
                        break;

                    case LogLevel.Error:
                        await _externalNotificationService.SendErrorAsync(externalNotificationMessage);
                        break;

                    case LogLevel.Critical:
                        await _externalNotificationService.SendCriticalAsync(externalNotificationMessage);
                        break;
                }
            }
        }
    }
}
