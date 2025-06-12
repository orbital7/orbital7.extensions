namespace Orbital7.Extensions.Data;

public static class NamedValueExtensions
{
    public static NamedValue<TId> ToNamedValue<TId>(
        this NamedId<TId> namedId)
    {
        return new NamedValue<TId>
        {
            Name = namedId.Name,
            Value = namedId.Id,
        };
    }

    public static List<NamedValue<TId>> ToNamedValueList<TId>(
        this IEnumerable<NamedId<TId>> list)
    {
        return list
            .Select(x => x.ToNamedValue())
            .ToList();
    }

    public static List<NamedValue<T>> ToNamedValueList<T>(
        this IEnumerable<T> list)
    {
        return list
            .Where(x => x != null)
            .Select(x => new NamedValue<T> 
            { 
                Name = x?.ToString(), 
                Value = x 
            })
            .ToList();
    }
}
