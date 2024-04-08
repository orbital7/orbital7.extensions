using Microsoft.Extensions.DependencyInjection;

namespace Orbital7.Extensions.Integrations.BetterStackApi;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddBetterStackLogs(
        this IServiceCollection services)
    {
        services.AddScoped<ILogsUploadService, LogsUploadService>(
            (serviceProvider) => new LogsUploadService(
                new BetterStackClient(
                    serviceProvider.GetRequiredService<IHttpClientFactory>())));

        return services;
    }

    public static IServiceCollection AddBetterStackUptime(
        this IServiceCollection services,
        string apiToken)
    {
        services.AddScoped<IUptimeHeartbeatsService, UptimeHeartbeatsService>(
            (serviceProvider) => new UptimeHeartbeatsService(
                new BetterStackClient(
                    serviceProvider.GetRequiredService<IHttpClientFactory>(),
                    apiToken)));

        return services;
    }
}
