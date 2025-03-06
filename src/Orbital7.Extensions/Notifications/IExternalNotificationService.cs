namespace Orbital7.Extensions.Notifications;

public interface IExternalNotificationService
{
    Task<bool> SendTraceAsync(
        string message);

    Task<bool> SendDebugAsync(
        string message);

    Task<bool> SendInformationAsync(
        string message);

    Task<bool> SendWarningAsync(
        string message);

    Task<bool> SendErrorAsync(
        string message);

    Task<bool> SendCriticalAsync(
        string message);
}
