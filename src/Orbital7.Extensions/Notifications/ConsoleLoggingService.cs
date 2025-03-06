using System.Runtime.CompilerServices;
using Microsoft.Extensions.Logging;

namespace Orbital7.Extensions.Notifications;

public class ConsoleLoggingService<TCategoryName> :
    ILoggingService
{
    public async Task LogTraceAsync(
        string message,
        IDictionary<string, object> metadata = null,
        [CallerMemberName] string callerMemberName = null,
        bool sendExternalNotification = false)
    {
        await LogAsync(LogLevel.Trace, message, sendExternalNotification);
    }

    public async Task LogDebugAsync(
        string message,
        IDictionary<string, object> metadata = null,
        [CallerMemberName] string callerMemberName = null,
        bool sendExternalNotification = false)
    {
        await LogAsync(LogLevel.Debug, message, sendExternalNotification);
    }

    public async Task LogInformationAsync(
        string message,
        IDictionary<string, object> metadata = null,
        [CallerMemberName] string callerMemberName = null,
        bool sendExternalNotification = false)
    {
        await LogAsync(LogLevel.Information, message, sendExternalNotification);
    }


    public async Task LogWarningAsync(
        string message,
        Exception exception = null,
        IDictionary<string, object> metadata = null,
        [CallerMemberName] string callerMemberName = null,
        bool sendExternalNotification = false)
    {
        await LogAsync(LogLevel.Warning, message, sendExternalNotification);
    }

    public async Task LogErrorAsync(
        string message,
        Exception exception = null,
        IDictionary<string, object> metadata = null,
        [CallerMemberName] string callerMemberName = null,
        bool sendExternalNotification = false)
    {
        await LogAsync(LogLevel.Error, message, sendExternalNotification);
    }

    public async Task LogCriticalAsync(
        string message,
        Exception exception = null,
        IDictionary<string, object> metadata = null,
        [CallerMemberName] string callerMemberName = null,
        bool sendExternalNotification = false)
    {
        await LogAsync(LogLevel.Critical, message, sendExternalNotification);
    }

    private async Task LogAsync(
        LogLevel logLevel,
        string message,
        bool sendExternalNotification)
    {
        var logger = typeof(TCategoryName).FullName;
        Console.WriteLine($"{DateTime.Now.ToDefaultDateTimeString()} [{logLevel.ToString().ToUpper()}] {logger}: {message}");

        await Task.CompletedTask;
    }
}
