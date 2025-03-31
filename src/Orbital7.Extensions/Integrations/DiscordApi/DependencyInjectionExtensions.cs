namespace Orbital7.Extensions.Integrations.DiscordApi;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddDiscordApi(
        this IServiceCollection services,
        string botToken)
    {
        services.AddScoped<IChannelsApi, ChannelsApi>(
            (serviceProvider) => new ChannelsApi(
                new DiscordApiClient(
                    serviceProvider.GetRequiredService<IHttpClientFactory>(),
                    botToken)));

        return services;
    }
}
