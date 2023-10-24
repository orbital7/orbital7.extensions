namespace Orbital7.Extensions.Integrations.BetterStackApi;

public interface IUptimeHeartbeatsService
{
    Task<HeartbeatsResponse> ListAllExistingAsync(
        int? page = null);

    Task<HeartbeatResponse> GetAsync(
        string id);

    Task<HeartbeatResponse> CreateAsync(
        HeartbeatRequest request);

    Task<HeartbeatResponse> UpdateAsync(
        string id,
        HeartbeatRequest request);

    Task DeleteAsync(
        string id);

    Task SendAsync(
        string url);
}
