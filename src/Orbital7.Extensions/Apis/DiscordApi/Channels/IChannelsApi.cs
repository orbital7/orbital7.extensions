namespace Orbital7.Extensions.Apis.DiscordApi.Channels;

public interface IChannelsApi
{
    Task<Message> CreateMessageAsync(
        ulong channelId,
        CreateMessageRequest request,
        CancellationToken cancellationToken = default);
}
