using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionConfigurationExtensions
    {
        public static IServiceCollection AddAppSettingsConfiguration(
            this IServiceCollection services,
            string environmentVariableName = "ASPNETCORE_ENVIRONMENT")
        {
            var builder = CreateConfigurationBuilder(environmentVariableName)
                    .AddEnvironmentVariables();
            services.AddSingleton<IConfiguration>(builder.Build());

            return services;
        }

        public static IServiceCollection AddAppSettingsConfigurationWithUserSecrets<TStartup>(
            this IServiceCollection services,
            string environmentVariableName = "ASPNETCORE_ENVIRONMENT")
            where TStartup : class
        {
            var builder = CreateConfigurationBuilder(environmentVariableName)
                .AddUserSecrets<TStartup>()
                .AddEnvironmentVariables();
            services.AddSingleton<IConfiguration>(builder.Build());

            return services;
        }

        private static IConfigurationBuilder CreateConfigurationBuilder(
                string environmentVariableName = "ASPNETCORE_ENVIRONMENT")
        {
            var environment = Environment.GetEnvironmentVariable(environmentVariableName);
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{environment}.json", optional: true);

            return builder;
        }
    }

    //public static class ConfigurationBuilderFactory
    //{
    //    public static IServiceCollection AppendConfiguration(
    //        IServiceCollection services,
    //        string environmentVariableName = "ASPNETCORE_ENVIRONMENT")
    //    {
    //        var builder = CreateConfigurationBuilder(environmentVariableName)
    //            .AddEnvironmentVariables();
    //        services.AddSingleton<IConfiguration>(builder.Build());

    //        return services;
    //    }

    //    public static IServiceCollection AppendConfigurationWithUserSecrets<TStartup>(
    //        IServiceCollection services,
    //        string environmentVariableName = "ASPNETCORE_ENVIRONMENT")
    //        where TStartup : class
    //    {
    //        var builder = CreateConfigurationBuilder(environmentVariableName)
    //            .AddUserSecrets<TStartup>()
    //            .AddEnvironmentVariables();
    //        services.AddSingleton<IConfiguration>(builder.Build());

    //        return services;
    //    }

    //    public static IConfigurationBuilder CreateConfigurationBuilder(
    //        string environmentVariableName = "ASPNETCORE_ENVIRONMENT")
    //    {
    //        var environment = Environment.GetEnvironmentVariable(environmentVariableName);
    //        var builder = new ConfigurationBuilder()
    //            .SetBasePath(Directory.GetCurrentDirectory())
    //            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
    //            .AddJsonFile($"appsettings.{environment}.json", optional: true);

    //        return builder;
    //    }
    //}
}
