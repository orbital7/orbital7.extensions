namespace Orbital7.Extensions.Apis.BetterStackApi.Uptime.Heartbeats;

public interface IHeartbeatsApi
{
    Task<ListHeartbeatsResponse> ListAsync(
        int? page = null,
        CancellationToken cancellationToken = default);

    Task<GetHeartbeatResponse> GetAsync(
        string id,
        CancellationToken cancellationToken = default);

    Task<GetHeartbeatResponse> CreateAsync(
        GetHeartbeatRequest request,
        CancellationToken cancellationToken = default);

    Task<GetHeartbeatResponse> UpdateAsync(
        string id,
        GetHeartbeatRequest request,
        CancellationToken cancellationToken = default);

    Task DeleteAsync(
        string id,
        CancellationToken cancellationToken = default);

    Task SendAsync(
        string url,
        CancellationToken cancellationToken = default);
}
