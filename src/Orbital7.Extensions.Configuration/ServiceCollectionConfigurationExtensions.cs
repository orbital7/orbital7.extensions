namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionConfigurationExtensions
{
    public static IServiceCollection AddConfiguration(
        this IServiceCollection services,
        string environmentVariableName = null,
        string[] args = null)
    {
        var builder = ConfigurationHelper.CreateConfigurationBuilder(
            environmentVariableName, 
            args);

        services.AddSingleton<IConfiguration>(builder.Build());

        return services;
    }

    public static IServiceCollection AddConfigurationWithUserSecrets<TAssemblyClass>(
        this IServiceCollection services,
        string environmentVariableName = null,
        string[] args = null)
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
        string environmentVariableName = null,
        string[] args = null)
        where TConfiguration : class
    {
        var configuration = ConfigurationHelper.GetConfiguration<TConfiguration>(
            environmentVariableName,
            args);

        services.AddSingleton<TConfiguration>(configuration);

        return services;
    }

    public static IServiceCollection AddConfiguration<TConfiguration, TAssemblyClass>(
        this IServiceCollection services,
        string environmentVariableName = null,
        string[] args = null)
        where TConfiguration : class
        where TAssemblyClass : class
    {
        var builder = ConfigurationHelper.CreateConfigurationBuilderWithUserSecrets<TAssemblyClass>(
            environmentVariableName,
            args);

        services.AddSingleton<IConfiguration>(builder.Build());

        return services;
    }
}