using System.Linq;

namespace System;

public static class GuidExtensions
{
    public static string ToString(this List<Guid> list, string delimiter, bool order, bool allowDuplicates, bool allowEmptyGuid)
    {
        var items = (from x in list where allowEmptyGuid || x != Guid.Empty select x);

        if (order)
            items = (from x in items orderby x select x);

        if (!allowDuplicates)
            items = items.Distinct();
        
        return items.ToList().ToString(delimiter);
    }
}
