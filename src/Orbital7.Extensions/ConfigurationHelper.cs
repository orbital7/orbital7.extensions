using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.UserSecrets;
using System.Dynamic;

namespace Orbital7.Extensions;

public static class ConfigurationHelper
{
    public const string DOTNET_ENVIRONMENT_VARIABLE = "DOTNET_ENVIRONMENT";

    public const string ASPNET_CORE_ENVIRONMENT_VARIABLE = "ASPNETCORE_ENVIRONMENT";

    public static IConfiguration GetConfiguration(
        string? environmentVariableName = null,
        string[]? args = null)
    {
        return CreateConfigurationBuilder(environmentVariableName, args)
            .Build();
    }

    public static IConfiguration GetConfigurationWithUserSecrets<TAssemblyClass>(
        string? environmentVariableName = null,
        string[]? args = null)
        where TAssemblyClass : class
    {
        return CreateConfigurationBuilderWithUserSecrets<TAssemblyClass>(environmentVariableName, args)
            .Build();
    }

    public static TConfiguration? GetConfiguration<TConfiguration>(
        string? environmentVariableName = null,
        string[]? args = null)
        where TConfiguration : class
    {
        return GetConfiguration(environmentVariableName, args)?
            .Get<TConfiguration>();
    }

    public static TConfiguration? GetConfigurationWithUserSecrets<TConfiguration, TAssemblyClass>(
        string? environmentVariableName = null,
        string[]? args = null)
        where TConfiguration : class
        where TAssemblyClass : class
    {
        return GetConfigurationWithUserSecrets<TAssemblyClass>(environmentVariableName, args)?
            .Get<TConfiguration>();
    }

    public static IConfigurationBuilder CreateConfigurationBuilder(
        string? environmentVariableName = null,
        string[]? args = null)
    {
        var builder = StartBuilder(environmentVariableName);
        return FinishBuilder(builder, true, args);
    }

    public static IConfigurationBuilder CreateConfigurationBuilderWithUserSecrets<TAssemblyClass>(
        string? environmentVariableName = null,
        string[]? args = null)
        where TAssemblyClass : class
    {
        var builder = StartBuilder(environmentVariableName);
        builder = builder.AddUserSecrets<TAssemblyClass>();
        return FinishBuilder(builder, true, args);
    }

    public static TConfiguration? ReadUserSecrets<TConfiguration, TAssemblyClass>()
        where TAssemblyClass : class
    {
        var secretsId = GetVerifiedSecretsId<TAssemblyClass>();

        var secretsFilePath = PathHelper.GetSecretsPathFromSecretsId(secretsId);
        if (Path.Exists(secretsFilePath))
        {
            return JsonSerializationHelper.DeserializeFromJsonFile<TConfiguration>(
                secretsFilePath);
        }
        else
        {
            return ReflectionHelper.CreateInstance<TConfiguration>();
        }
    }

    public static TConfiguration WriteUserSecrets<TConfiguration, TAssemblyClass>(
        TConfiguration userSecrets)
        where TConfiguration : class
        where TAssemblyClass : class
    {
        var secretsFilePath = PathHelper.GetSecretsPathFromSecretsId(
            GetVerifiedSecretsId<TAssemblyClass>());

        JsonSerializationHelper.SerializeToJsonFile(
            userSecrets,
            secretsFilePath,
            ignoreNullValues: false,
            indentFormatting: true);

        return userSecrets;
    }

    public static dynamic UpdateUserSecretsDynamically<TAssemblyClass>(
        Action<dynamic> updateAction)
        where TAssemblyClass : class
    {
        // Read the existing secrets file.
        var secretsId = GetVerifiedSecretsId<TAssemblyClass>();

        // Ensure secrets path exists.
        var secretsFilePath = PathHelper.GetSecretsPathFromSecretsId(secretsId);
        if (Path.Exists(secretsFilePath))
        {
            dynamic secrets = JsonSerializationHelper.DeserializeFromJsonFile<ExpandoObject>(
                secretsFilePath);

            // Execute the provided update action.
            updateAction?.Invoke(secrets);

            // Overwrite the file with changes.
            JsonSerializationHelper.SerializeToJsonFile(
                secrets,
                secretsFilePath,
                ignoreNullValues: false,
                indentFormatting: true);

            return secrets;
        }
        else
        {
            throw new Exception($"Secrets file '{secretsFilePath}' does not exist");
        }
    }

    public static string? GetUserSecretsId<TAssemblyClass>()
    {
        return Assembly.GetAssembly(typeof(TAssemblyClass))?
            .GetCustomAttribute<UserSecretsIdAttribute>()?
            .UserSecretsId;
    }

    private static string GetVerifiedSecretsId<TAssemblyClass>()
    {
        var secretsId = GetUserSecretsId<TAssemblyClass>();
        if (secretsId.HasText())
        {
            return secretsId;
        }
        else
        {
            throw new Exception($"The specified assembly class '{typeof(TAssemblyClass).Name}' does not contain a UserSecretsId.");
        }
    }

    private static IConfigurationBuilder StartBuilder(
        string? environmentVariableName)
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

        if (environmentVariableName.HasText())
        {
            var environment = Environment.GetEnvironmentVariable(environmentVariableName);
            builder.AddJsonFile($"appsettings.{environment}.json", optional: true, reloadOnChange: true);
        }

        return builder;
    }

    private static IConfigurationBuilder FinishBuilder(
        IConfigurationBuilder builder,
        bool addEnvironmentVariables,
        string[]? args)
    {
        if (addEnvironmentVariables)
        {
            builder.AddEnvironmentVariables();
        }

        if (args != null)
        {
            builder.AddCommandLine(args);
        }

        return builder;
    }
}
