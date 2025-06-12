namespace Orbital7.Extensions.Integrations.BetterStackApi;

// Documentation:
//  https://betterstack.com/docs/uptime/api/getting-started-with-uptime-api/
//  https://betterstack.com/docs/uptime/api/list-all-existing-hearbeats/
public class UptimeHeartbeatsApi :
    UptimeApiBase, IUptimeHeartbeatsApi
{
    private const string BASE_ROUTE = "heartbeats";

    public UptimeHeartbeatsApi(
        IBetterStackApiClient client) : 
        base(client)
    {

    }

    public async Task<HeartbeatsResponse> ListAllExistingAsync(
        int? page = null,
        CancellationToken cancellationToken = default)
    {
        var url = BuildRequestUrl(BASE_ROUTE);
        if (page.HasValue)
        {
            url += $"?page={page}";
        }

        return await this.Client.SendGetRequestAsync<HeartbeatsResponse>(
            url,
            cancellationToken);
    }

    public async Task<HeartbeatResponse> GetAsync(
        string id,
        CancellationToken cancellationToken = default)
    {
        return await this.Client.SendGetRequestAsync<HeartbeatResponse>(
            BuildRequestUrl($"{BASE_ROUTE}/{id}"),
            cancellationToken);
    }

    public async Task<HeartbeatResponse> CreateAsync(
        HeartbeatRequest request,
        CancellationToken cancellationToken = default)
    {
        return await this.Client.SendPostRequestAsync<HeartbeatRequest, HeartbeatResponse>(
            BuildRequestUrl(BASE_ROUTE),
            request,
            cancellationToken);
    }

    public async Task<HeartbeatResponse> UpdateAsync(
        string id,
        HeartbeatRequest request,
        CancellationToken cancellationToken = default)
    {
        return await this.Client.SendPatchRequestAsync<HeartbeatRequest, HeartbeatResponse>(
            BuildRequestUrl($"{BASE_ROUTE}/{id}"),
            request,
            cancellationToken);
    }

    public async Task DeleteAsync(
        string id,
        CancellationToken cancellationToken = default)
    {
        await this.Client.SendDeleteRequestAsync<string>(
            BuildRequestUrl($"{BASE_ROUTE}/{id}"),
            cancellationToken);
    }

    public async Task SendAsync(
        string url,
        CancellationToken cancellationToken = default)
    {
        await this.Client.SendPostRequestAsync<string>(
            url, 
            cancellationToken);
    }
}
