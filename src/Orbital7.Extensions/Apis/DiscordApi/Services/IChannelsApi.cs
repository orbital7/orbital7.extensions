namespace Orbital7.Extensions.Apis.DiscordApi;

public interface IChannelsApi
{
    Task<Message> CreateMessageAsync(
        ulong channelId,
        CreateMessageRequest request,
        CancellationToken cancellationToken = default);
}
