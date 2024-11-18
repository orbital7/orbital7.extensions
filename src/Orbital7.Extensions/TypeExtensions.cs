namespace System;

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
               type == typeof(decimal) || type == typeof(decimal?) ||
               type == typeof(float) || type == typeof(float?) ||
               type == typeof(double) || type == typeof(double?) ||
               type == typeof(short) || type == typeof(short?);
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
}
