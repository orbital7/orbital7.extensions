using System.Runtime.CompilerServices;

namespace Orbital7.Extensions.Notifications;

public interface ILoggingService
{
    Task LogAsync(
        LogLevel logLevel,
        string message,
        Exception? exception = null,
        IDictionary<string, object?>? metadata = null,
        [CallerMemberName] string? callerMemberName = null,
        bool sendExternalNotification = false,
        bool includeExternalNotificationDetails = true);
}
