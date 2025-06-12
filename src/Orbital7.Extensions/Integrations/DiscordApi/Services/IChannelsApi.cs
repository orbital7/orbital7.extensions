namespace Orbital7.Extensions.Integrations.DiscordApi;

public interface IChannelsApi
{
    Task<Message> CreateMessageAsync(
        ulong channelId,
        CreateMessageRequest request,
        CancellationToken cancellationToken = default);
}
