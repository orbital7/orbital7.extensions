namespace System;

public static class NamedIdExtensions
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
}
