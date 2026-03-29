namespace Orbital7.Extensions.Apis.BetterStackApi.Uptime.Heartbeats;

// Documentation:
//  https://betterstack.com/docs/uptime/api/getting-started-with-uptime-api/
//  https://betterstack.com/docs/uptime/api/list-all-existing-hearbeats/
public class HeartbeatsApi :
    UptimeApiBase, IHeartbeatsApi
{
    private const string ENDPOINT_PATH = "heartbeats";

    public HeartbeatsApi(
        IBetterStackApiClient client) : 
        base(client)
    {

    }

    public async Task<ListHeartbeatsResponse> ListAsync(
        int? page = null,
        CancellationToken cancellationToken = default)
    {
        var query = CreateQueryStringCollection();
        if (page.HasValue)
        {
            query["page"] = page.Value.ToString();
        }

        var url = BuildRequestUrl(ENDPOINT_PATH, query);

        return await this.Client.SendGetRequestAsync<ListHeartbeatsResponse>(
            url,
            cancellationToken);
    }

    public async Task<HeartbeatResponse> GetAsync(
        string id,
        CancellationToken cancellationToken = default)
    {
        return await this.Client.SendGetRequestAsync<HeartbeatResponse>(
            BuildRequestUrl($"{ENDPOINT_PATH}/{id}"),
            cancellationToken);
    }

    public async Task<HeartbeatResponse> CreateAsync(
        HeartbeatRequest request,
        CancellationToken cancellationToken = default)
    {
        return await this.Client.SendPostRequestAsync<HeartbeatRequest, HeartbeatResponse>(
            BuildRequestUrl(ENDPOINT_PATH),
            request,
            cancellationToken);
    }

    public async Task<HeartbeatResponse> UpdateAsync(
        string id,
        HeartbeatRequest request,
        CancellationToken cancellationToken = default)
    {
        return await this.Client.SendPatchRequestAsync<HeartbeatRequest, HeartbeatResponse>(
            BuildRequestUrl($"{ENDPOINT_PATH}/{id}"),
            request,
            cancellationToken);
    }

    public async Task DeleteAsync(
        string id,
        CancellationToken cancellationToken = default)
    {
        await this.Client.SendDeleteRequestAsync<string>(
            BuildRequestUrl($"{ENDPOINT_PATH}/{id}"),
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
