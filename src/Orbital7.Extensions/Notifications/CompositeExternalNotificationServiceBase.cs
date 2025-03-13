namespace Orbital7.Extensions.Notifications;

public abstract class CompositeExternalNotificationServiceBase :
    IExternalNotificationService
{
    protected readonly List<IExternalNotificationService> _externalNotificationServices = new();

    public virtual async Task<bool> SendAsync(
        LogLevel logLevel, 
        string message)
    {
        bool notificationsSent = false;

        foreach (var externalNotificationService in _externalNotificationServices)
        {
            bool shouldSend = ShouldSendTo(
                externalNotificationService,
                logLevel,
                message);

            if (shouldSend)
            {
                var notificationSent = await externalNotificationService.SendAsync(
                    logLevel,
                    message);

                notificationsSent = notificationsSent || notificationSent;
            }
        }

        return notificationsSent;
    }

    protected void AddExternalNotificationService(
        IExternalNotificationService externalNotificationService)
    {
        if (externalNotificationService != null)
        {
            _externalNotificationServices.Add(externalNotificationService);
        }
    }

    protected virtual bool ShouldSendTo(
        IExternalNotificationService externalNotificationService,
        LogLevel logLevel,
        string message)
    {
        return true;
    }
}
