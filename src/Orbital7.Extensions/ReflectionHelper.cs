using Orbital7.Extensions.Data;

namespace Orbital7.Extensions;

public static class ReflectionHelper
{
    public static object? GetPropertyValue<T>(
        T? obj,
        string propertyName)
    {
        object? objValue = default;

        var propertyInfo = obj?
            .GetType()
            .GetProperty(propertyName);

        if (propertyInfo != null)
        {
            objValue = propertyInfo.GetValue(obj, null);
        }

        return objValue;
    }

    public static List<PropertyValue> GetPropertyValues<T>(
        T obj)
    {
        PropertyInfo[] properties = typeof(T).GetProperties();

        return properties
            .Select(x => new PropertyValue()
            {
                Name = x.Name,
                DisplayName = x.GetDisplayName(),
                Value = x.GetValue(obj),
            })
            .OrderBy(x => x.DisplayName)
            .ToList();
    }

    public static List<PropertyValue> GetPropertyValues<T>(
        T obj,
        params Expression<Func<T, object>>[] properties)
        where T : class
    {
        var list = new List<PropertyValue>();

        foreach (var property in properties)
        {
            var memberInfo = property.Body.GetMemberInfo();
            if (memberInfo != null)
            {
                list.Add(new PropertyValue()
                {
                    Name = memberInfo.Name,
                    DisplayName = memberInfo.GetDisplayName(),
                    Value = property.Compile().Invoke(obj),
                });
            }
        }

        return list
            .OrderBy(x => x.DisplayName)
            .ToList();
    }

    public static List<PropertyValue> GetPropertyValues<T>(
        T obj,
        params (Expression<Func<T, object>>, string)[] propertiesAndDisplayNameOverrides)
        where T : class
    {
        var list = new List<PropertyValue>();

        foreach (var property in propertiesAndDisplayNameOverrides)
        {
            var memberInfo = property.Item1.Body.GetMemberInfo();
            if (memberInfo != null)
            {
                list.Add(new PropertyValue()
                {
                    Name = property.Item1.Name!,

                    DisplayName = property.Item2.HasText() ?
                        property.Item2 :
                        memberInfo.GetDisplayName(),

                    Value = property.Item1
                        .Compile()
                        .Invoke(obj),
                });
            }
        }

        return list;
    }

    public static string? GetPropertyDisplayName<T>(
        Expression<Func<T, object>> property)
        where T : class
    {
        var memberInfo = property.Body.GetMemberInfo();
        return memberInfo?.GetDisplayName();
    }

    public static string? GetExecutingAssemblyFolderPath()
    {
        string location = Assembly.GetExecutingAssembly().Location;
        UriBuilder uri = new UriBuilder(location);
        string path = Uri.UnescapeDataString(uri.Path);
        return Path.GetDirectoryName(path);
    }

    public static List<T> CreateExternalInstances<T>()
    {
        var folderPath = GetExecutingAssemblyFolderPath();

        if (folderPath.HasText())
        {
            return typeof(T)
                .GetExternalTypes()
                .CreateInstances<T>();
        }

        return [];
    }

    public static List<T> CreateExternalInstances<T>(
        string assembliesFolderPath)
    {
        return typeof(T)
            .GetExternalTypes(assembliesFolderPath)
            .CreateInstances<T>();
    }

    public static T? CreateInstance<T>(
        string assemblyQualifiedTypeName, 
        object[]? parameters = null)
    {
        return CreateInstance<T>(
            Type.GetType(assemblyQualifiedTypeName), 
            parameters);
    }

    public static T? CreateInstance<T>(
        string assemblyName, 
        string typeName, 
        object[]? parameters = null)
    {
        return CreateInstance<T>(
            Assembly.Load(assemblyName).GetType(typeName), 
            parameters);
    }

    public static T? CreateInstance<T>(
        object[]? parameters = null)
    {
        return CreateInstance<T>(typeof(T), parameters);
    }

    public static List<T> CreateInstancesWithAttribute<T, TAttribute>()
        where TAttribute : Attribute
    {
        var types = Assembly
            .GetExecutingAssembly()
            .GetTypesWithAttribute<T, TAttribute>();

        return types
            .Select(x => x.CreateInstance<T>())
            .Where(x => x != null)
            .ToList()!;
    }

    private static T? CreateInstance<T>(
        Type? type, 
        object[]? parameters)
    {
        if (type != null)
        {
            return type.CreateInstance<T>(parameters);
        }
        else
        {
            return default;
        }
    }
}
