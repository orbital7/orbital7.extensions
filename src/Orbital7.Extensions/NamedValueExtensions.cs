namespace System;

public static class NamedValueExtensions
{
    public static NamedValue<TId> ToNamedValue<TId>(
        this NamedId<TId> namedId)
    {
        return new NamedValue<TId>(namedId.Name, namedId.Id);
    }

    public static List<NamedValue<TId>> ToNamedValueList<TId>(
        this List<NamedId<TId>> list)
    {
        return list
            .Select(x => x.ToNamedValue())
            .ToList();
    }

    public static List<NamedValue<T>> ToNamedValueList<T>(
        this List<T> list)
    {
        return list
            .Select(x => new NamedValue<T>(x.ToString(), x))
            .ToList();
    }
}
