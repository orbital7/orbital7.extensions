﻿namespace Orbital7.Extensions;

public static class EnumExtensions
{
    public static string ToDisplayString(
        this Enum value)
    {
        var attributes = value.GetAttributes<DisplayAttribute>();
        if (attributes != null && attributes.Any())
        {
            return attributes.First().Name ?? value.ToString().PascalCaseToPhrase();
        }
        else
        {
            return value.ToString().PascalCaseToPhrase();
        }  
    }

    public static TAttribute[] GetAttributes<TAttribute>(
        this Enum value)
        where TAttribute : Attribute
    {
        if (value != null)
        {
            var fieldInfo = value.GetType().GetRuntimeField(value.ToString());
            if (fieldInfo != null)
            {
                var attributes = fieldInfo.GetCustomAttributes(
                    typeof(TAttribute), false) as TAttribute[];
                if (attributes != null && attributes.Any())
                {
                    return attributes;
                }
            }
        }

        return null;
    }
}
