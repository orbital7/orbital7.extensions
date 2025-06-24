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
        T? obj)
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

    public static List<PropertyValue> GetFormattedPropertyValues<T>(
        T obj,
        TimeConverter? timeConverter,
        params PropertyFormatter<T>[] propertyFormatters)
        where T : class
    {
        var list = new List<PropertyValue>();

        foreach (var propertyFormatter in propertyFormatters)
        {
            var options = new DisplayValueOptions();
            propertyFormatter.ConfigureDisplayValueOptions?.Invoke(options);

            var value = propertyFormatter.Property.Compile().Invoke(obj);
            var memberInfo = propertyFormatter.Property.Body.GetMemberInfo();

            list.Add(new PropertyValue()
            {
                Name = propertyFormatter.Property.Name,

                DisplayName = propertyFormatter.DisplayName.HasText() ?
                    propertyFormatter.DisplayName :
                    memberInfo?.GetDisplayName(),

                Value = propertyFormatter.GetDisplayValue != null ?
                    propertyFormatter.GetDisplayValue.Invoke(obj, timeConverter, options) : 
                    memberInfo != null ?
                        memberInfo.GetDisplayValue(
                            value,
                            timeConverter,
                            options: options) :
                        value.GetDisplayValue(
                            timeConverter,
                            options: options),
            });
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

    // This method takes an object parameter so the type can be inferred.
    public static string? GetPropertyDisplayName<T>(
        T? obj,
        Expression<Func<T, object>> property)
        where T : class
    {
        return GetPropertyDisplayName(property);
    }

    public static string GetExecutingAssemblyFolderPath()
    {
        string location = Assembly.GetExecutingAssembly().Location;
        UriBuilder uri = new UriBuilder(location);
        var filePath = Uri.UnescapeDataString(uri.Path);

        var folderPath = Path.GetDirectoryName(filePath);
        if (folderPath.HasText())
        {
            return folderPath;
        }
        else
        {
            throw new Exception(
                "Unable to determine the folder path of the executing assembly");
        }
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
