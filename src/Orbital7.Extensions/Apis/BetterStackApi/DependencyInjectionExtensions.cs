namespace Orbital7.Extensions.Apis.BetterStackApi;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddBetterStackTelemetryApi(
        this IServiceCollection services)
    {
        services.AddScoped<ITelemetryLoggingApi, TelemetryLoggingApi>(
            (serviceProvider) => new TelemetryLoggingApi(
                new BetterStackApiClient(
                    serviceProvider.GetRequiredService<IHttpClientFactory>())));

        return services;
    }

    public static IServiceCollection AddBetterStackUptimeApi(
        this IServiceCollection services,
        string? apiToken)
    {
        if (apiToken.HasText())
        {
            services.AddScoped<IUptimeHeartbeatsApi, UptimeHeartbeatsApi>(
                (serviceProvider) => new UptimeHeartbeatsApi(
                    new BetterStackApiClient(
                        serviceProvider.GetRequiredService<IHttpClientFactory>(),
                        apiToken)));
        }

        return services;
    }
}
