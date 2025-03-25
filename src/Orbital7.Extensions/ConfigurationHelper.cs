using Microsoft.Extensions.Configuration.UserSecrets;

namespace Microsoft.Extensions.Configuration;

public static class ConfigurationHelper
{
    public const string DOTNET_ENVIRONMENT_VARIABLE = "DOTNET_ENVIRONMENT";

    public const string ASPNET_CORE_ENVIRONMENT_VARIABLE = "ASPNETCORE_ENVIRONMENT";

    public static IConfiguration GetConfiguration(
        string environmentVariableName = null,
        string[] args = null)
    {
        return CreateConfigurationBuilder(environmentVariableName, args)
            .Build();
    }

    public static IConfiguration GetConfigurationWithUserSecrets<TAssemblyClass>(
        string environmentVariableName = null,
        string[] args = null)
        where TAssemblyClass : class
    {
        return CreateConfigurationBuilderWithUserSecrets<TAssemblyClass>(environmentVariableName, args)
            .Build();
    }

    public static TConfiguration GetConfiguration<TConfiguration>(
        string environmentVariableName = null,
        string[] args = null)
        where TConfiguration : class
    {
        return GetConfiguration(environmentVariableName, args)
            .Get<TConfiguration>();
    }

    public static TConfiguration GetConfigurationWithUserSecrets<TConfiguration, TAssemblyClass>(
        string environmentVariableName = null,
        string[] args = null)
        where TConfiguration : class
        where TAssemblyClass : class
    {
        return GetConfigurationWithUserSecrets<TAssemblyClass>(environmentVariableName, args)
            .Get<TConfiguration>();
    }

    public static IConfigurationBuilder CreateConfigurationBuilder(
        string environmentVariableName = null,
        string[] args = null)
    {
        var builder = StartBuilder(environmentVariableName);
        return FinishBuilder(builder, true, args);
    }

    public static IConfigurationBuilder CreateConfigurationBuilderWithUserSecrets<TAssemblyClass>(
        string environmentVariableName = null,
        string[] args = null)
        where TAssemblyClass : class
    {
        var builder = StartBuilder(environmentVariableName);
        builder = builder.AddUserSecrets<TAssemblyClass>();
        return FinishBuilder(builder, true, args);
    }

    public static TConfiguration ReadUserSecrets<TConfiguration, TAssemblyClass>()
        where TAssemblyClass : class
    {
        var secretsFilePath = GetUserSecretsFilePath<TAssemblyClass>();
        return JsonSerializationHelper.DeserializeFromJsonFile<TConfiguration>(secretsFilePath);
    }

    public static TConfiguration WriteUserSecrets<TConfiguration, TAssemblyClass>(
        TConfiguration userSecrets)
        where TConfiguration : class
        where TAssemblyClass : class
    {
        var secretsFilePath = GetUserSecretsFilePath<TAssemblyClass>();
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
        // NOTE: This method uses Newtonsoft.Json, which we don't normally
        // use, but appears to be included in this project via a dependent
        // assembly, so fuck it, we're going for it.

        // Read the existing secrets JSON.
        var secretsFilePath = GetUserSecretsFilePath<TAssemblyClass>();
        var secretsJson = File.ReadAllText(secretsFilePath);
        dynamic secrets = Newtonsoft.Json.JsonConvert
            .DeserializeObject<System.Dynamic.ExpandoObject>(
                secretsJson, 
                new Newtonsoft.Json.Converters.ExpandoObjectConverter());

        // Execute the provided update action.
        updateAction?.Invoke(secrets);

        // Overwrite the file with changes.
        var updatedSecretsJson = Newtonsoft.Json.JsonConvert
            .SerializeObject(
                secrets,
                Newtonsoft.Json.Formatting.Indented);
        File.WriteAllText(
            secretsFilePath, 
            updatedSecretsJson);

        return secrets;
    }

    public static string GetUserSecretsFilePath<TAssemblyClass>()
    {
        var secretsId = Assembly.GetAssembly(typeof(TAssemblyClass))
            .GetCustomAttribute<UserSecretsIdAttribute>()
            .UserSecretsId;

        var secretsFilePath = PathHelper.GetSecretsPathFromSecretsId(secretsId);

        return secretsFilePath;
    }

    private static IConfigurationBuilder StartBuilder(
        string environmentVariableName)
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
        string[] args)
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
