namespace Orbital7.Extensions.Apis.BetterStack;

public class UptimeHeartbeatsService :
    UptimeServiceBase, IUptimeHeartbeatsService
{
    private const string BASE_ROUTE = "heartbeats";

    public UptimeHeartbeatsService(
        IBetterStackClient client) : 
        base(client)
    {

    }

    public async Task<HeartbeatsResponse> ListAllExistingAsync(
        int? page = null)
    {
        var url = BuildRequestUrl(BASE_ROUTE);
        if (page.HasValue)
        {
            url += $"?page={page}";
        }

        return await this.Client.SendGetRequestAsync<HeartbeatsResponse>(
            url);
    }

    public async Task<HeartbeatResponse> GetAsync(
        string id)
    {
        return await this.Client.SendGetRequestAsync<HeartbeatResponse>(
            BuildRequestUrl($"{BASE_ROUTE}/{id}"));
    }

    public async Task<HeartbeatResponse> CreateAsync(
        HeartbeatRequest request)
    {
        return await this.Client.SendPostRequestAsync<HeartbeatRequest, HeartbeatResponse>(
            BuildRequestUrl(BASE_ROUTE),
            request);
    }

    public async Task<HeartbeatResponse> UpdateAsync(
        string id,
        HeartbeatRequest request)
    {
        return await this.Client.SendPatchRequestAsync<HeartbeatRequest, HeartbeatResponse>(
            BuildRequestUrl($"{BASE_ROUTE}/{id}"),
            request);
    }

    public async Task DeleteAsync(
        string id)
    {
        await this.Client.SendDeleteRequestAsync<string>(
            BuildRequestUrl($"{BASE_ROUTE}/{id}"));
    }
}
