using System.Runtime.CompilerServices;

namespace Orbital7.Extensions.Notifications;

public interface ILoggingService
{
    Task LogTraceAsync(
        string message,
        IDictionary<string, object> metadata = null,
        [CallerMemberName] string callerMemberName = null,
        bool sendExternalNotification = false);

    Task LogDebugAsync(
        string message,
        IDictionary<string, object> metadata = null,
        [CallerMemberName] string callerMemberName = null,
        bool sendExternalNotification = false);

    Task LogInformationAsync(
        string message,
        IDictionary<string, object> metadata = null,
        [CallerMemberName] string callerMemberName = null,
        bool sendExternalNotification = false);

    Task LogWarningAsync(
        string message,
        Exception exception = null,
        IDictionary<string, object> metadata = null,
        [CallerMemberName] string callerMemberName = null,
        bool sendExternalNotification = false);

    Task LogErrorAsync(
        string message,
        Exception exception = null,
        IDictionary<string, object> metadata = null,
        [CallerMemberName] string callerMemberName = null,
        bool sendExternalNotification = false);

    Task LogCriticalAsync(
        string message,
        Exception exception = null,
        IDictionary<string, object> metadata = null,
        [CallerMemberName] string callerMemberName = null,
        bool sendExternalNotification = false);
}
