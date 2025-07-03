namespace Orbital7.Extensions.Apis.SlackApi;

public interface IChatApi
{
    Task<PostMessageResponse> PostMessageAsync(
        PostMessageRequest request,
        CancellationToken cancellationToken = default);
}
