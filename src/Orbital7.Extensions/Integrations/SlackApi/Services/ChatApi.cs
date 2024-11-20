﻿namespace Orbital7.Extensions.Integrations.SlackApi;

// Documentation: https://api.slack.com/messaging/sending
public class ChatApi :
    SlackApiBase, IChatApi
{
    public ChatApi(
        ISlackApiClient client) :
        base(client)
    {

    }

    public async Task<PostMessageResponse> PostMessageAsync(
        PostMessageRequest request)
    {
        return await this.Client.SendPostRequestAsync<PostMessageRequest, PostMessageResponse>(
            this.BuildRequestUrl("chat.postMessage"),
            request);
    }
}