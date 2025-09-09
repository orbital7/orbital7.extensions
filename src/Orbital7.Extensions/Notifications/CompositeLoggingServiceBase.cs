using System.Runtime.CompilerServices;

namespace Orbital7.Extensions.Notifications;

public abstract class CompositeLoggingServiceBase :
    ILoggingService
{
    protected readonly List<ILoggingService> _loggingServices = new();

    public virtual async Task LogAsync(
        LogLevel logLevel, 
        string message, 
        Exception? exception = null, 
        IDictionary<string, object?>? metadata = null, 
        [CallerMemberName] string? callerMemberName = null, 
        bool sendExternalNotification = false,
        bool includeExternalNotificationDetails = true)
    {
        foreach (var loggingService in _loggingServices)
        {
            bool shouldLog = ShouldLogTo(
                loggingService,
                logLevel,
                message,
                exception,
                metadata,
                callerMemberName,
                sendExternalNotification);

            if (shouldLog)
            {
                await loggingService.LogAsync(
                    logLevel,
                    message,
                    exception,
                    metadata,
                    callerMemberName,
                    sendExternalNotification);
            }
        }
    }

    protected virtual void AddLoggingService(
        ILoggingService loggingService)
    {
        if (loggingService != null)
        {
            _loggingServices.Add(loggingService);
        }
    }

    protected virtual bool ShouldLogTo(
        ILoggingService loggingService,
        LogLevel logLevel,
        string message,
        Exception? exception,
        IDictionary<string, object?>? metadata,
        string? callerMemberName,
        bool sendExternalNotification)
    {
        return true;
    }
}
