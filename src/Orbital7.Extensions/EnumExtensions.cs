using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace System;

public static class EnumExtensions
{
    public static string ToDisplayString(this Enum value, string nullValue = "")
    {
        if (value == null)
            return nullValue;

        var fieldInfo = value.GetType().GetRuntimeField(value.ToString());
        if (fieldInfo == null)
            return nullValue;

        var descriptionAttributes = fieldInfo.GetCustomAttributes(
            typeof(DisplayAttribute), false) as DisplayAttribute[];

        if (descriptionAttributes == null) return value.ToString();
        return (descriptionAttributes.Length > 0) ? descriptionAttributes[0].Name : value.ToString();
    }
}
