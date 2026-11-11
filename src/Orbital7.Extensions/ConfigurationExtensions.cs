using Microsoft.Extensions.Configuration;

namespace Orbital7.Extensions;

public static class Extensions
{
    public static string GetRequiredStringValue(
        this IConfiguration configuration,
        string key)
    {
        return configuration[key] ?? 
            throw new ArgumentNullException(key);
    }

    public static ulong GetRequiredULongValue(
        this IConfiguration configuration,
        string key)
    {
        return StringHelper.ParseULong(
            configuration.GetRequiredStringValue(key));
    }
}
