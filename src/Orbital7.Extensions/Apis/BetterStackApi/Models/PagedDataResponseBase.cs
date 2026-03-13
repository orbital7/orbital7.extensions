namespace Orbital7.Extensions.Apis.BetterStackApi;

public abstract record PagedDataResponseBase<TData>
{
    [JsonPropertyName("data")]
    public TData[]? Data { get; init; }

    [JsonPropertyName("pagination")]
    public Pagination? Pagination { get; init; }

    public int ParsePageIndex(
        string url)
    {
        var uri = new Uri(url);
        var parameters = System.Web.HttpUtility.ParseQueryString(uri.Query);
        return Convert.ToInt32(parameters["page"]);
    }
}
