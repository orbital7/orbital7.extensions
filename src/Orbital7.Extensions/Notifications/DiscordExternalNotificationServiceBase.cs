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

    public virtual async Task<bool> SendTraceAsync(
        string message)
    {
        return await SendAsync(this.TraceChannelId, message);
    }

    public virtual async Task<bool> SendDebugAsync(
        string message)
    {
        return await SendAsync(this.DebugChannelId, message);
    }

    public virtual async Task<bool> SendInformationAsync(
        string message)
    {
        return await SendAsync(this.InformationChannelId, message);
    }

    public virtual async Task<bool> SendWarningAsync(
        string message)
    {
        return await SendAsync(this.WarningChannelId, message);
    }

    public virtual async Task<bool> SendErrorAsync(
        string message)
    {
        return await SendAsync(this.ErrorChannelId, message);
    }

    public virtual async Task<bool> SendCriticalAsync(
        string message)
    {
        return await SendAsync(this.CriticalChannelId, message);
    }

    protected virtual async Task<bool> SendAsync(
        ulong channelId,
        string message)
    {
        try
        {
            // Ensure we're logged in.
            if (_discordRestClient.LoginState != Discord.LoginState.LoggedIn)
            { 
                await _discordRestClient.LoginAsync(
                    TokenType.Bot,
                    this.BotToken);
            }

            // Get the channel.
            var channel = await _discordRestClient.GetChannelAsync(channelId) as IMessageChannel;
            if (channel != null)
            {
                // TODO: Unclear what happens here when sending a message fails.
                var result = await channel.SendMessageAsync(message);
                return result != null;
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
}
