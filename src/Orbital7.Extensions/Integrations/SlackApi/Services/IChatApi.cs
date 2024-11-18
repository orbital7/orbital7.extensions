namespace Orbital7.Extensions.Integrations.SlackApi;

public interface IChatApi
{
    Task<PostMessageResponse> PostMessageAsync(
        PostMessageRequest request);
}
