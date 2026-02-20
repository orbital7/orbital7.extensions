namespace Orbital7.Extensions.Apis.DiscordApi;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddDiscordApi(
        this IServiceCollection services,
        string? botToken,
        string? httpClientName = HttpClientFactoryHelper.HTTP_CLIENT_NAME_TIMEOUT_20S)
    {
        if (botToken.HasText())
        {
            services.AddSingleton<IChannelsApi, ChannelsApi>(
                (serviceProvider) => new ChannelsApi(
                    new DiscordApiClient(
                        serviceProvider.GetRequiredService<IHttpClientFactory>(),
                        botToken: botToken,
                        httpClientName: httpClientName)));
        }

        return services;
    }
}
