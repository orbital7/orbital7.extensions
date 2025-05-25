using Orbital7.Extensions.Data;

namespace Orbital7.Extensions;

public static class EnumHelper
{
    public static List<TEnum> ToValueList<TEnum>()
    {
        var enumType = EnsureEnumType<TEnum>();
        Array enumValArray = Enum.GetValues(enumType);
        List<TEnum> enumValList = new List<TEnum>(enumValArray.Length);
        foreach (int val in enumValArray)
            enumValList.Add((TEnum)Enum.Parse(enumType, val.ToString()));

        return enumValList;
    }

    public static List<NamedValue<TEnum>> ToNamedValueList<TEnum>()
    {
        var list = new List<NamedValue<TEnum>>();
        foreach (TEnum item in ToValueList<TEnum>())
        {
            list.Add(new NamedValue<TEnum>()
            {
                Name = (item as Enum)!.ToDisplayString(),
                Value = item,
            });
        }

        return list;
    }

    public static TEnum Parse<TEnum>(
        string value)
    {
        return (TEnum)Enum.Parse(typeof(TEnum), value);
    }

    public static TEnum? ParseNullable<TEnum>(
        string? value,
        TEnum? defaultValue = default)
    {
        if (value.HasText())
        {
            return (TEnum)Enum.Parse(typeof(TEnum), value);
        }

        return defaultValue;
    }

    private static Type EnsureEnumType<TEnum>()
    {
        Type enumType = typeof(TEnum).GetBaseType();

        // Can't use generic type constraints on value types,    
        // so have to do check like this    .
        if (!enumType.IsBaseOrNullableEnumType())
            throw new ArgumentException("TEnum must be of type System.Enum");

        return enumType;
    }
}
