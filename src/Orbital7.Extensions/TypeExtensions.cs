namespace Orbital7.Extensions;

public static class TypeExtensions
{
    public static bool IsBaseOrNullableTemporalType(
        this Type type)
    {
        return type == typeof(DateTime) || type == typeof(DateTime?) ||
               type == typeof(DateOnly) || type == typeof(DateOnly?) ||
               type == typeof(TimeOnly) || type == typeof(TimeOnly?);
    }

    public static bool IsBaseOrNullableNumericType(
        this Type type)
    {
        return type == typeof(int) || type == typeof(int?) ||
               type == typeof(long) || type == typeof(long?) ||
               type == typeof(uint) || type == typeof(uint?) ||
               type == typeof(ulong) || type == typeof(ulong?) ||
               type == typeof(decimal) || type == typeof(decimal?) ||
               type == typeof(float) || type == typeof(float?) ||
               type == typeof(double) || type == typeof(double?) ||
               type == typeof(short) || type == typeof(short?) ||
               type == typeof(ushort) || type == typeof(ushort?);
    }

    public static bool IsBaseOrNullableEnumType(
        this Type type)
    {
        return type.IsEnum ||
            (Nullable.GetUnderlyingType(type)?.IsEnum ?? false);
    }

    public static bool IsNullableType(
        this Type type)
    {
        return Nullable.GetUnderlyingType(type) != null;
    }

    public static Type GetBaseType(
        this Type type)
    {
        var nullableUnderlyingType = Nullable.GetUnderlyingType(type);
        if (nullableUnderlyingType != null)
        {
            return nullableUnderlyingType;
        }
        else
        {
            return type;
        }
    }
    public static IEnumerable<PropertyInfo> GetPropertiesContainingAttribute(
        this Type type,
        Type attributeType)
    {
        return type.GetRuntimeProperties().Where(prop => prop.IsDefined(attributeType));
    }

    public static IEnumerable<(PropertyInfo, TAttribute)> GetPropertyAttributePairs<TAttribute>(
        this Type type)
        where TAttribute : Attribute
    {
        return from p in type.GetRuntimeProperties()
               let attr = p.GetCustomAttributes(typeof(TAttribute), true).ToList()
               where attr.Count == 1
               select ((p, attr.First() as TAttribute));
    }

    public static string GetPropertyDisplayName(
        this Type type,
        string propertyName)
    {
        object? displayName = GetPropertyAttributeValue(
            type,
            propertyName,
            typeof(DisplayAttribute), "Name");

        if (displayName != null)
            return displayName.ToString() ?? propertyName;
        else
            return propertyName;
    }

    public static (PropertyInfo?, TAttribute?) GetPropertyAttribute<TAttribute>(
        this Type type,
        string propertyName)
        where TAttribute : Attribute
    {
        Type attributeType = typeof(TAttribute);
        var propertyInfo = type.GetRuntimeProperty(propertyName);

        if (propertyInfo != null && propertyInfo.IsDefined(attributeType))
            return (propertyInfo, propertyInfo.GetCustomAttribute(attributeType) as TAttribute);

        return (null, null);
    }

    public static object? GetPropertyAttributeValue(
        this Type objectType,
        string propertyName,
        Type attributeType,
        string attributePropertyName)
    {
        var propertyInfo = objectType.GetRuntimeProperty(propertyName);
        if (propertyInfo != null)
        {
            if (propertyInfo.IsDefined(attributeType))
            {
                var attributeInstance = propertyInfo.GetCustomAttribute(attributeType);
                if (attributeInstance != null)
                    foreach (PropertyInfo info in attributeType.GetRuntimeProperties())
                        if (info.CanRead && string.Compare(info.Name, attributePropertyName, StringComparison.OrdinalIgnoreCase) == 0)
                            return info.GetValue(attributeInstance, null);
            }
        }

        return null;
    }
}
