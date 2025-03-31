using Orbital7.Extensions.Data;

namespace Orbital7.Extensions;

public static class EnumHelper
{
    public static List<TEnum> ToValueList<TEnum>()
    {
        Type enumType = typeof(TEnum).GetBaseType();

        // Can't use generic type constraints on value types,    
        // so have to do check like this    
        if (!enumType.IsBaseOrNullableEnumType())
            throw new ArgumentException("TEnum must be of type System.Enum");
        Array enumValArray = Enum.GetValues(enumType);
        List<TEnum> enumValList = new List<TEnum>(enumValArray.Length);
        foreach (int val in enumValArray)
            enumValList.Add((TEnum)Enum.Parse(enumType, val.ToString()));

        return enumValList;
    }

    public static List<NamedValue<TEnum>> ToNamedValueList<TEnum>()
    {
        var list = new List<NamedValue<TEnum>>();

        foreach (var item in ToValueList<TEnum>())
        {
            list.Add(new NamedValue<TEnum>((item as Enum).ToDisplayString(), item));
        }

        return list;
    }

    public static TEnum Parse<TEnum>(
        string value)
    {
        return (TEnum)Enum.Parse(typeof(TEnum), value);
    }
}
