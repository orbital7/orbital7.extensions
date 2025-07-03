using Microsoft.Extensions.Configuration;
using System.Data;

namespace Orbital7.Extensions;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddDisplayValueOptions(
        this IServiceCollection services,
        Action<DisplayValueOptionsBuilder>? configureBuilder = null)
    {
        services.AddTransient(provider =>
        {
            var builder = new DisplayValueOptionsBuilder();
            configureBuilder?.Invoke(builder);
            return builder;
        });

        services.AddTransient(provider =>
        {
            var builder = provider.GetRequiredService<DisplayValueOptionsBuilder>();
            return builder.Build();
        });

        return services;
    }

    public static IServiceCollection AddConfiguration(
        this IServiceCollection services,
        string? environmentVariableName = null,
        string[]? args = null)
    {
        var builder = ConfigurationHelper.CreateConfigurationBuilder(
            environmentVariableName, 
            args);

        services.AddSingleton<IConfiguration>(builder.Build());

        return services;
    }

    public static IServiceCollection AddConfigurationWithUserSecrets<TAssemblyClass>(
        this IServiceCollection services,
        string? environmentVariableName = null,
        string[]? args = null)
        where TAssemblyClass : class
    {
        var builder = ConfigurationHelper.CreateConfigurationBuilderWithUserSecrets<TAssemblyClass>(
            environmentVariableName, 
            args);

        services.AddSingleton<IConfiguration>(builder.Build());

        return services;
    }

    public static IServiceCollection AddConfiguration<TConfiguration>(
        this IServiceCollection services,
        string? environmentVariableName = null,
        string[]? args = null)
        where TConfiguration : class, new()
    {
        var configuration = ConfigurationHelper.GetConfiguration<TConfiguration>(
            environmentVariableName,
            args);

        if (configuration != null)
        {
            services.AddSingleton(configuration);
        }

        return services;
    }

    public static IServiceCollection AddConfiguration<TConfiguration, TAssemblyClass>(
        this IServiceCollection services,
        string? environmentVariableName = null,
        string[]? args = null)
        where TConfiguration : class
        where TAssemblyClass : class
    {
        var builder = ConfigurationHelper.CreateConfigurationBuilderWithUserSecrets<TAssemblyClass>(
            environmentVariableName,
            args);

        services.AddSingleton<IConfiguration>(builder.Build());

        return services;
    }

    public static IServiceCollection Remove<T>(
        this IServiceCollection services)
    {
        if (services.IsReadOnly)
        {
            throw new ReadOnlyException($"{nameof(services)} is read only");
        }

        var serviceDescriptor = services
            .FirstOrDefault(descriptor => descriptor.ServiceType == typeof(T));

        if (serviceDescriptor != null)
        {
            services.Remove(serviceDescriptor);
        }

        return services;
    }
}