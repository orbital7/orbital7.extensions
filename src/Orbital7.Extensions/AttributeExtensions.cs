﻿using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;

namespace System.Reflection;

public static class AttributeExtensions
{
    public static IEnumerable<PropertyInfo> GetPropertiesWithAttribute(
        this Type objectType, 
        Type attributeType)
    {
        return objectType.GetRuntimeProperties().Where(prop => prop.IsDefined(attributeType));
    }

    public static IEnumerable<(PropertyInfo, T)> GetPropertiesWithAttribute<T>(
        this Type objectType) 
        where T : Attribute
    {
        return from p in objectType.GetRuntimeProperties()
               let attr = p.GetCustomAttributes(typeof(T), true).ToList()
               where attr.Count == 1
               select ((p, attr.First() as T));
    }

    public static string GetPropertyDisplayName(
        this Type objectType, 
        string propertyName)
    {
        object displayName = GetPropertyAttributeValue(
            objectType, 
            propertyName, 
            typeof(DisplayAttribute), "Name");

        if (displayName != null)
            return displayName.ToString();
        else
            return propertyName;
    }

    public static DisplayAttribute GetDisplayAttribute(
        this MemberInfo memberInfo)
    {
        if (memberInfo == null)
        {
            throw new ArgumentException(
                "No property reference expression was found.",
                "propertyExpression");
        }

        return memberInfo.GetAttribute<DisplayAttribute>(false);
    }

    public static string GetDisplayName(
        this MemberInfo memberInfo)
    {
        return memberInfo.GetDisplayAttribute()?.Name ??
            memberInfo.Name?.PascalCaseToPhrase();
    }

    public static T GetPropertyAttribute<T>(
        this Type objectType, 
        string propertyName) 
        where T : Attribute
    {
        Type attributeType = typeof(T);
        var propertyInfo = objectType.GetRuntimeProperty(propertyName);

        if (propertyInfo != null && propertyInfo.IsDefined(attributeType))
            return propertyInfo.GetCustomAttribute(attributeType) as T;

        return null;
    }

    public static T GetAttribute<T>(
        this MemberInfo member, 
        bool isRequired)
        where T : Attribute
    {
        var attribute = member.GetCustomAttributes(typeof(T), false).SingleOrDefault();

        if (attribute == null && isRequired)
        {
            throw new ArgumentException(
                string.Format(
                    CultureInfo.InvariantCulture,
                    "The {0} attribute must be defined on member {1}",
                    typeof(T).Name,
                    member.Name));
        }

        return (T)attribute;
    }

    public static bool HasAttribute<T>(
        this MemberInfo member)
        where T : Attribute
    {
        return member.GetAttribute<T>(false) != null;
    }

    public static List<Type> GetTypesWithAttribute<TAttribute>(
        this Assembly assembly,
        Type objectType)
        where TAttribute : Attribute
    {
        var types = assembly.GetTypes(objectType);
        var attributeType = typeof(TAttribute);

        return (from x in types
                let a = x.GetCustomAttribute(attributeType) as TAttribute
                where a != null
                select x).ToList();
    }

    public static List<Type> GetTypesWithAttribute<TObject, TAttribute>(
        this Assembly assembly)
        where TAttribute : Attribute
    {
        return assembly.GetTypesWithAttribute<TAttribute>(typeof(TObject));
    }

    public static List<(Type, TAttribute)> GetTypeAttributePairs<TAttribute>(
        this Assembly assembly, 
        Type objectType,
        bool includeNullAttributes = false) 
        where TAttribute : Attribute
    {
        var types = assembly.GetTypes(objectType);
        var attributeType = typeof(TAttribute);

        return (from x in types
                let a = x.GetCustomAttribute(attributeType) as TAttribute
                where includeNullAttributes || a != null
                select (x, a)).ToList();
    }

    public static object GetPropertyAttributeValue(
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
                        if (info.CanRead && string.Compare(info.Name, attributePropertyName, StringComparison.CurrentCultureIgnoreCase) == 0)
                            return info.GetValue(attributeInstance, null);
            }
        }

        return null;
    }
}
