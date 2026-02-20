namespace Orbital7.Extensions.Apis.SlackApi;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddSlackApi(
        this IServiceCollection services,
        string? apiToken,
        string? httpClientName = HttpClientFactoryHelper.HTTP_CLIENT_NAME_TIMEOUT_20S)
    {
        if (apiToken.HasText())
        {
            services.AddSingleton<IChatApi, ChatApi>(
                (serviceProvider) => new ChatApi(
                    new SlackApiClient(
                        serviceProvider.GetRequiredService<IHttpClientFactory>(),
                        bearerToken: apiToken,
                        httpClientName: httpClientName)));
        }

        return services;
    }
}
