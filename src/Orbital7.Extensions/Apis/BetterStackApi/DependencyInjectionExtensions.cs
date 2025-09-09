namespace Orbital7.Extensions.Apis.BetterStackApi;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddBetterStackTelemetryApi(
        this IServiceCollection services,
        string? httpClientName = HttpClientFactoryHelper.HTTP_CLIENT_NAME_TIMEOUT_20S)
    {
        services.AddScoped<ITelemetryLoggingApi, TelemetryLoggingApi>(
            (serviceProvider) => new TelemetryLoggingApi(
                new BetterStackApiClient(
                    serviceProvider.GetRequiredService<IHttpClientFactory>(),
                    httpClientName: httpClientName)));

        return services;
    }

    public static IServiceCollection AddBetterStackUptimeApi(
        this IServiceCollection services,
        string? apiToken,
        string? httpClientName = HttpClientFactoryHelper.HTTP_CLIENT_NAME_TIMEOUT_20S)
    {
        if (apiToken.HasText())
        {
            services.AddScoped<IUptimeHeartbeatsApi, UptimeHeartbeatsApi>(
                (serviceProvider) => new UptimeHeartbeatsApi(
                    new BetterStackApiClient(
                        serviceProvider.GetRequiredService<IHttpClientFactory>(),
                        bearerToken: apiToken,
                        httpClientName: httpClientName)));
        }

        return services;
    }
}
