using Orbital7.Extensions.Integrations.SlackApi;

namespace Orbital7.Extensions.Notifications;

public abstract class SlackExternalNotificationServiceBase :
    IExternalNotificationService
{
    private readonly IChatApi _chatApi;

    protected abstract string TraceChannel { get; }

    protected abstract string DebugChannel { get; }

    protected abstract string InformationChannel { get; }

    protected abstract string WarningChannel { get; }

    protected abstract string ErrorChannel { get; }

    protected abstract string CriticalChannel { get; }

    protected SlackExternalNotificationServiceBase(
        IChatApi chatApi)
    {
        _chatApi = chatApi;
    }

    public virtual async Task<bool> SendTraceAsync(
        string message)
    {
        return await SendAsync(this.TraceChannel, message);
    }

    public virtual async Task<bool> SendDebugAsync(
        string message)
    {
        return await SendAsync(this.DebugChannel, message);
    }

    public virtual async Task<bool> SendInformationAsync(
        string message)
    {
        return await SendAsync(this.InformationChannel, message);
    }

    public virtual async Task<bool> SendWarningAsync(
        string message)
    {
        return await SendAsync(this.WarningChannel, message);
    }

    public virtual async Task<bool> SendErrorAsync(
        string message)
    {
        return await SendAsync(this.ErrorChannel, message);
    }

    public virtual async Task<bool> SendCriticalAsync(
        string message)
    {
        return await SendAsync(this.CriticalChannel, message);
    }

    protected virtual async Task<bool> SendAsync(
        string channel, 
        string message)
    {
        try
        {
            var request = new PostMessageRequest()
            {
                Channel = channel,
                Text = message,
            };

            var response = await _chatApi.PostMessageAsync(request);
            response.AssertOk();
            return true;
        }
        catch (Exception)
        {
            // TODO: What to do here? We don't want to send it to the 
            // logging service, else we have a circular dependency. It's 
            // more important that the logging service sends to the external
            // notification service than we send to the logging service here.
            // We want the logging service to be the source of truth.
            return false;
        }
    }
}
