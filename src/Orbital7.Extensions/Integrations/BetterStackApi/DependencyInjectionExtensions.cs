using Microsoft.Extensions.DependencyInjection;

namespace Orbital7.Extensions.Integrations.BetterStackApi;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddBetterStackLogs(
        this IServiceCollection services)
    {
        services.AddScoped<ILogsUploadApi, LogsUploadApi>(
            (serviceProvider) => new LogsUploadApi(
                new BetterStackApiClient(
                    serviceProvider.GetRequiredService<IHttpClientFactory>())));

        return services;
    }

    public static IServiceCollection AddBetterStackUptime(
        this IServiceCollection services,
        string apiToken)
    {
        services.AddScoped<IUptimeHeartbeatsApi, UptimeHeartbeatsApi>(
            (serviceProvider) => new UptimeHeartbeatsApi(
                new BetterStackApiClient(
                    serviceProvider.GetRequiredService<IHttpClientFactory>(),
                    apiToken)));

        return services;
    }
}
