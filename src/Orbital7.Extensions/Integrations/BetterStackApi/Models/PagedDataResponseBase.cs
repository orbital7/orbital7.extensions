namespace Orbital7.Extensions.Integrations.BetterStackApi;

public abstract class PagedDataResponseBase<TData>
{
    [JsonPropertyName("data")]
    public TData[]? Data { get; set; }

    [JsonPropertyName("pagination")]
    public Pagination? Pagination { get; set; }

    public int ParsePageIndex(
        string url)
    {
        var uri = new Uri(url);
        var parameters = System.Web.HttpUtility.ParseQueryString(uri.Query);
        return Convert.ToInt32(parameters["page"]);
    }
}
