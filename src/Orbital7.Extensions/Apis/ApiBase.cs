using System.Collections.Specialized;
using System.Web;

namespace Orbital7.Extensions.Apis;

public abstract class ApiBase<TClient> 
    where TClient : IApiClient
{
    protected TClient Client { get; private init; }

    protected abstract string BaseUrl { get; }

    protected ApiBase(
        TClient client)
    {
        this.Client = client;
    }

    protected virtual NameValueCollection CreateQueryStringCollection(
        string? existingQueryString = null)
    {
        return HttpUtility.ParseQueryString(
            existingQueryString ?? string.Empty);
    }

    protected string BuildRequestUrl(
        string endpointPath,
        NameValueCollection? queryStringCollection = null)
    {
        var url = $"{this.BaseUrl.PruneEnd("/")}/{endpointPath.PruneStart("/")}";

        var uriBuilder = new UriBuilder(url);

        var query = HttpUtility.ParseQueryString(
            uriBuilder.Query);

        if (queryStringCollection != null)
        {
            foreach (string key in queryStringCollection)
            {
                query[key] = queryStringCollection[key];
            }
        }

        uriBuilder.Query = query.ToString() ?? string.Empty;
        return uriBuilder.ToString();
    }
}
