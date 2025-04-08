using System.Runtime.CompilerServices;

namespace Orbital7.Extensions.Notifications;

public class ConsoleLoggingService<TCategoryName> :
    ILoggingService
{
    public virtual Task LogAsync(
        LogLevel logLevel,
        string message,
        Exception? exception = null,
        IDictionary<string, object?>? metadata = null,
        [CallerMemberName] string? callerMemberName = null,
        bool sendExternalNotification = false)
    {
        var logger = typeof(TCategoryName).FullName;
        Console.WriteLine($"{DateTime.Now.ToDefaultDateTimeString()} [{logLevel.ToString().ToUpper()}] {logger}: {message}");

        return Task.CompletedTask;
    }
}
