using Discord;
using Discord.Rest;

namespace Orbital7.Extensions.Notifications;

public abstract class DiscordExternalNotificationServiceBase :
    IExternalNotificationService, IDisposable, IAsyncDisposable
{
    private readonly DiscordRestClient _discordRestClient = new();

    protected abstract string BotToken { get; }

    protected abstract ulong TraceChannelId { get; }

    protected abstract ulong DebugChannelId { get; }

    protected abstract ulong InformationChannelId { get; }

    protected abstract ulong WarningChannelId { get; }

    protected abstract ulong ErrorChannelId { get; }

    protected abstract ulong CriticalChannelId { get; }

    public void Dispose()
    {
        _discordRestClient.Dispose();
    }

    public async ValueTask DisposeAsync()
    {
        await _discordRestClient.DisposeAsync();
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
            // Only attempt login if we have a token and a channel ID.
            if (this.BotToken.HasText() && channelId > 0)
            {
                // Ensure we're logged in.
                if (_discordRestClient.LoginState != LoginState.LoggedIn)
                {
                    await _discordRestClient.LoginAsync(
                        TokenType.Bot,
                        this.BotToken);
                }

                // Get the channel.
                var channel = await _discordRestClient.GetChannelAsync(channelId) as IMessageChannel;
                if (channel != null)
                {
                    // Clean the message.
                    var cleanedMessage = CleanMessage(message);

                    // TODO: Unclear what happens here when sending a message fails.
                    var result = await channel.SendMessageAsync(cleanedMessage);
                    return result != null;
                }
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
