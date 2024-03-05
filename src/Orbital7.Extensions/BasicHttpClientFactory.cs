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
            this.LazyHttpMessageHandler.Value, 
            disposeHandler: false);
    }

    public void Dispose()
    {
        if (this.LazyHttpMessageHandler.IsValueCreated)
        {
            this.LazyHttpMessageHandler.Value.Dispose();
        }
    }
}
