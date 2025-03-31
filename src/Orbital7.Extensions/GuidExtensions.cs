namespace Orbital7.Extensions;

public static class GuidExtensions
{
    public static string ToShortString(
        this Guid guid)
    {
        if (guid == Guid.Empty)
        {
            return null;
        }
        else
        {
            return GuidFactory.ToShortString(guid);
        }
    }

    public static string ToShortString(
        this Guid? guid)
    {
        if (guid == null)
        {
            return null;
        }
        else
        {
            return guid.Value.ToShortString();
        }
    }

    public static string ToString(
        this List<Guid> list, 
        string delimiter, 
        bool order, 
        bool allowDuplicates, 
        bool allowEmpty)
    {
        var query = (from x in list 
                     where allowEmpty || x != Guid.Empty 
                     select x);

        if (order)
        {
            query = (from x in query
                     orderby x
                     select x);
        }

        if (!allowDuplicates)
        {
            query = query.Distinct();
        }
        
        return query
            .ToList()
            .ToString(delimiter);
    }
}
