namespace Orbital7.Extensions;

public static class HttpClientFactoryHelper
{
    public const string HTTP_CLIENT_NAME_TIMEOUT_10S = "HttpClientTimeout10s";

    public const string HTTP_CLIENT_NAME_TIMEOUT_20S = "HttpClientTimeout20s";

    public const string HTTP_CLIENT_NAME_TIMEOUT_30S = "HttpClientTimeout30s";

    public const string HTTP_CLIENT_NAME_TIMEOUT_60S = "HttpClientTimeout60s";

    public const string HTTP_CLIENT_NAME_TIMEOUT_120S = "HttpClientTimeout120s";

    public static IServiceCollection AddNamedHttpClientsWithTimeouts(
        this IServiceCollection services)
    {
        services.AddHttpClient(
            HTTP_CLIENT_NAME_TIMEOUT_10S, 
            httpClient =>
            {
                httpClient.Timeout = TimeSpan.FromSeconds(10);
            });

        services.AddHttpClient(
            HTTP_CLIENT_NAME_TIMEOUT_20S, 
            httpClient =>
            {
                httpClient.Timeout = TimeSpan.FromSeconds(20);
            });

        services.AddHttpClient(
            HTTP_CLIENT_NAME_TIMEOUT_30S, 
            httpClient =>
            {
                httpClient.Timeout = TimeSpan.FromSeconds(30);
            });

        services.AddHttpClient(
            HTTP_CLIENT_NAME_TIMEOUT_60S, 
            httpClient =>
            {
                httpClient.Timeout = TimeSpan.FromSeconds(60);
            });

        services.AddHttpClient(
            HTTP_CLIENT_NAME_TIMEOUT_120S, 
            httpClient =>
            {
                httpClient.Timeout = TimeSpan.FromSeconds(120);
            });

        return services;
    }
}
