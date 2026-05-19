namespace Orbital7.Extensions;

public static class StringListExtensions
{     
    public static List<T> ToTypedList<T>(
        this IEnumerable<string> list)
    {
        var typedList = new List<T>();

        foreach (var item in list)
        {
            if (item.HasText())
            {
                var value = item.ToTypedValue<T>();
                if (value != null)
                {
                    typedList.Add(value);
                }
            }
        }

        return typedList;
    }

    public static List<T?> ToNullableTypedList<T>(
        this IEnumerable<string> list)
    {
        var typedList = new List<T?>();

        foreach (var item in list)
        {
            typedList.Add(item.ToTypedValue<T?>());
        }

        return typedList;
    }

    public static bool HasItemThatContains(
        this IEnumerable<string>? list,
        string? value,
        StringComparison stringComparison = StringComparison.OrdinalIgnoreCase)
    {
        return list != null &&
            value.HasText() &&
            list.Any(x => x.Contains(value, stringComparison));
    }

    public static bool HasItemThatStartsWith(
        this IEnumerable<string>? list,
        string? value,
        StringComparison stringComparison = StringComparison.OrdinalIgnoreCase)
    {
        return list != null && 
            value.HasText() &&
            list.Any(x => x.StartsWith(value, stringComparison));
    }

    public static bool HasItemThatEndsWith(
        this IEnumerable<string>? list,
        string? value,
        StringComparison stringComparison = StringComparison.OrdinalIgnoreCase)
    {
        return list != null &&
            value.HasText() &&
            list.Any(x => x.EndsWith(value, stringComparison));
    }
}
