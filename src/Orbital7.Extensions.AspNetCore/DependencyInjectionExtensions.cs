namespace Orbital7.Extensions.AspNetCore;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddOrbital7BlazorServices(
        this IServiceCollection services)
    {
        services.AddScoped<TimeConverter, TimeConverter>();
        services.AddScoped<IMessenger, WeakReferenceMessenger>();
        services.AddBlazoredModal();

        return services;
    }
}
