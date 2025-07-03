namespace Orbital7.Extensions.Apis.BetterStackApi;

public interface IUptimeHeartbeatsApi
{
    Task<HeartbeatsResponse> ListAllExistingAsync(
        int? page = null,
        CancellationToken cancellationToken = default);

    Task<HeartbeatResponse> GetAsync(
        string id,
        CancellationToken cancellationToken = default);

    Task<HeartbeatResponse> CreateAsync(
        HeartbeatRequest request,
        CancellationToken cancellationToken = default);

    Task<HeartbeatResponse> UpdateAsync(
        string id,
        HeartbeatRequest request,
        CancellationToken cancellationToken = default);

    Task DeleteAsync(
        string id,
        CancellationToken cancellationToken = default);

    Task SendAsync(
        string url,
        CancellationToken cancellationToken = default);
}
