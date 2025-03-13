namespace Orbital7.Extensions.Notifications;

public class ConsoleExternalNotificationService :
    IExternalNotificationService
{
    public Task<bool> SendAsync(
        LogLevel logLevel,
        string message)
    {
        if (logLevel != LogLevel.None)
        {
            Console.WriteLine($"{GetChannel(logLevel)}: {message}");
        }

        return Task.FromResult(true);
    }

    protected virtual string GetChannel(
        LogLevel logLevel)
    {
        return logLevel switch
        {
            LogLevel.Trace => "#trace",
            LogLevel.Debug => "#debug",
            LogLevel.Information => "#information",
            LogLevel.Warning => "#warning",
            LogLevel.Error => "#error",
            LogLevel.Critical => "#critical",
            _ => throw new ArgumentOutOfRangeException(nameof(logLevel), logLevel, null)
        };
    }
}
