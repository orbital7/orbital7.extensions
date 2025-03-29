namespace Orbital7.Extensions.Integrations.DiscordApi;

public class ChannelsApi(
    IDiscordApiClient client) :
    DiscordApiBase(client), IChannelsApi
{
    // https://discord.com/developers/docs/resources/message#create-message
    public async Task<Message> CreateMessageAsync(
        ulong channelId, 
        CreateMessageRequest request)
    {
        return await this.Client.SendPostRequestAsync<CreateMessageRequest, Message>(
            this.BuildRequestUrl($"channels/{channelId}/messages"),
            request);
    }
}
