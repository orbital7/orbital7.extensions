using Orbital7.Extensions.Integrations.DiscordApi;

namespace Orbital7.Extensions.Notifications;

public abstract class DiscordExternalNotificationServiceBase :
    IExternalNotificationService
{
    private readonly IChannelsApi _channelsApi;

    protected abstract ulong TraceChannelId { get; }

    protected abstract ulong DebugChannelId { get; }

    protected abstract ulong InformationChannelId { get; }

    protected abstract ulong WarningChannelId { get; }

    protected abstract ulong ErrorChannelId { get; }

    protected abstract ulong CriticalChannelId { get; }

    protected DiscordExternalNotificationServiceBase(
        IChannelsApi channelsApi)
    {
        _channelsApi = channelsApi;
    }

    public virtual async Task<bool> SendAsync(
        LogLevel logLevel,
        string message)
    {
        if (logLevel == LogLevel.None)
        {
            return false;
        }
        else
        {
            var channelId = GetChannelId(logLevel);
            return await ExecuteSendAsync(channelId, message);
        }
    }

    protected virtual ulong GetChannelId(
        LogLevel logLevel)
    {
        return logLevel switch
        {
            LogLevel.Trace => this.TraceChannelId,
            LogLevel.Debug => this.DebugChannelId,
            LogLevel.Information => this.InformationChannelId,
            LogLevel.Warning => this.WarningChannelId,
            LogLevel.Error => this.ErrorChannelId,
            LogLevel.Critical => this.CriticalChannelId,
            _ => throw new ArgumentOutOfRangeException(nameof(logLevel), logLevel, null)
        };
    }

    protected virtual async Task<bool> ExecuteSendAsync(
        ulong channelId,
        string message)
    {
        try
        {
            // Only send if we have a channel.
            if (channelId > 0)
            {
                var cleanedMessage = CleanMessage(message);

                var request = new CreateMessageRequest()
                {
                    Content = cleanedMessage,
                };

                var result = await _channelsApi.CreateMessageAsync(
                    channelId,
                    request);

                return result?.Id.HasText() ?? false;
            }
        }
        catch (Exception)
        {
            // TODO: What to do here? We don't want to send it to the 
            // logging service, else we have a circular dependency. It's 
            // more important that the logging service sends to the external
            // notification service than we send to the logging service here.
            // We want the logging service to be the source of truth.
        }

        return false;
    }

    protected virtual string CleanMessage(
        string message)
    {
        // Discord double line spaces are huge, so take them out.
        message = message.Replace(
            $"{IExternalNotificationService.MSG_LINE_TERM}{IExternalNotificationService.MSG_LINE_TERM}",
            $"{IExternalNotificationService.MSG_LINE_TERM}");

        return message;
    }
}
