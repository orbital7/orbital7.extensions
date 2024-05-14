namespace System.Net.Http;

public class BasicHttpClientFactory :
    IHttpClientFactory, IDisposable
{
    private Lazy<HttpMessageHandler> LazyHttpMessageHandler { get; set; } =
        new Lazy<HttpMessageHandler>(() => new HttpClientHandler());

    public HttpClient CreateClient(
        string name)
    {
        return new HttpClient(
            LazyHttpMessageHandler.Value,
            disposeHandler: false);
    }

    public void Dispose()
    {
        if (LazyHttpMessageHandler.IsValueCreated)
        {
            LazyHttpMessageHandler.Value.Dispose();
        }
    }
}
