namespace Orbital7.Extensions.Notifications;

public interface IExternalNotificationService
{
    public const string MSG_LINE_TERM = "\n";

    Task<bool> SendAsync(
        LogLevel logLevel,
        string message);
}
