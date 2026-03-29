namespace Orbital7.Extensions.Apis.SlackApi.Chat;

public interface IChatApi
{
    Task<PostMessageResponse> PostMessageAsync(
        PostMessageRequest request,
        CancellationToken cancellationToken = default);
}
