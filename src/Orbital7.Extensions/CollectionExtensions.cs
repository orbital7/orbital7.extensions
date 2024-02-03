using System.Collections;

namespace System;

public static class CollectionExtensions
{
    public static bool HasItems(
        this ICollection collection)
    {
        return collection.Count > 0;
    }

    public static bool IsEmpty(
        this ICollection collection)
    {
        return !collection.HasItems();
    }
}
