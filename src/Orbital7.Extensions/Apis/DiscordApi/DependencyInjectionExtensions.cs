namespace Orbital7.Extensions.Apis.DiscordApi;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddDiscordApi(
        this IServiceCollection services,
        string? botToken)
    {
        if (botToken.HasText())
        {
            services.AddScoped<IChannelsApi, ChannelsApi>(
                (serviceProvider) => new ChannelsApi(
                    new DiscordApiClient(
                        serviceProvider.GetRequiredService<IHttpClientFactory>(),
                        botToken)));
        }

        return services;
    }
}
