namespace Orbital7.Extensions.Apis.SlackApi;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddSlackApi(
        this IServiceCollection services,
        string? apiToken)
    {
        if (apiToken.HasText())
        {
            services.AddScoped<IChatApi, ChatApi>(
                (serviceProvider) => new ChatApi(
                    new SlackApiClient(
                        serviceProvider.GetRequiredService<IHttpClientFactory>(),
                        apiToken)));
        }

        return services;
    }
}
