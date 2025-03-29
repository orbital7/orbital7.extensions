namespace Orbital7.Extensions.Integrations.SlackApi;

// Documentation: https://api.slack.com/messaging/sending
public class ChatApi(
    ISlackApiClient client) :
    SlackApiBase(client), IChatApi
{
    public async Task<PostMessageResponse> PostMessageAsync(
        PostMessageRequest request)
    {
        return await this.Client.SendPostRequestAsync<PostMessageRequest, PostMessageResponse>(
            this.BuildRequestUrl("chat.postMessage"),
            request);
    }
}
