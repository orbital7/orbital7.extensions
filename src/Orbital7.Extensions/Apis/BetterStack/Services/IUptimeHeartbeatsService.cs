namespace Orbital7.Extensions.Apis.BetterStack;

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
}
