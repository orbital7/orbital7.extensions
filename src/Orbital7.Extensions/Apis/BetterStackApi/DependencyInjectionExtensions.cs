using Orbital7.Extensions.Apis.BetterStackApi.Telemetry;
using Orbital7.Extensions.Apis.BetterStackApi.Uptime.Heartbeats;

namespace Orbital7.Extensions.Apis.BetterStackApi;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddBetterStackTelemetryApi(
        this IServiceCollection services,
        string? httpClientName = HttpClientFactoryHelper.HTTP_CLIENT_NAME_TIMEOUT_20S)
    {
        services.AddSingleton<ILoggingApi, LoggingApi>(
            (serviceProvider) => new LoggingApi(
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
            services.AddSingleton<IHeartbeatsApi, HeartbeatsApi>(
                (serviceProvider) => new HeartbeatsApi(
                    new BetterStackApiClient(
                        serviceProvider.GetRequiredService<IHttpClientFactory>(),
                        bearerToken: apiToken,
                        httpClientName: httpClientName)));
        }

        return services;
    }
}
