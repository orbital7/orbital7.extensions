using Orbital7.Extensions.Apis.SlackApi;

namespace Orbital7.Extensions.Notifications;

public abstract class SlackExternalNotificationServiceBase :
    IExternalNotificationService
{
    public const string PLATFORM = "Slack";

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
            var channel = GetChannel(logLevel);
            return await ExecuteSendAsync(channel, message);
        }
    }

    protected virtual string GetChannel(
        LogLevel logLevel)
    {
        return logLevel switch
        {
            LogLevel.Trace => this.TraceChannel,
            LogLevel.Debug => this.DebugChannel,
            LogLevel.Information => this.InformationChannel,
            LogLevel.Warning => this.WarningChannel,
            LogLevel.Error => this.ErrorChannel,
            LogLevel.Critical => this.CriticalChannel,
            _ => throw new ArgumentOutOfRangeException(nameof(logLevel), logLevel, null)
        };
    }

    protected virtual async Task<bool> ExecuteSendAsync(
        string channel, 
        string message)
    {
        try
        {
            // Only send if we have a channel.
            if (channel.HasText())
            {
                var cleanedMessage = CleanMessage(message);

                var request = new PostMessageRequest()
                {
                    Channel = channel,
                    Text = cleanedMessage,
                };

                var response = await _chatApi.PostMessageAsync(request);
                response.AssertOk();
                return true;
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
        return message
            .Replace( // Try to add nice looking bullets.
                $"{IExternalNotificationService.MSG_LINE_TERM}* ",
                $"{IExternalNotificationService.MSG_LINE_TERM}  •  ")
            .Replace("**", "*")     // Convert from Discord markdown to Slack markdown.
            .Replace("__", "_");    // Convert from Discord markdown to Slack markdown.
    }
}
