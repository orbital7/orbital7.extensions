﻿namespace Orbital7.Extensions;

public static class GenericListExtensions
{
    public static int Replace<T>(
        this IList<T> source, 
        T oldValue, 
        T newValue)
    {
        if (source == null)
            throw new ArgumentNullException(nameof(source));

        var index = source.IndexOf(oldValue);
        if (index != -1)
            source[index] = newValue;
        return index;
    }

    public static void ReplaceAll<T>(
        this IList<T> source, 
        T oldValue, 
        T newValue)
    {
        if (source == null)
            throw new ArgumentNullException(nameof(source));

        int index = -1;
        do
        {
            index = source.IndexOf(oldValue);
            if (index != -1)
                source[index] = newValue;
        } while (index != -1);
    }


    public static IEnumerable<T> Replace<T>(
        this IEnumerable<T> source, 
        T oldValue, 
        T newValue)
    {
        if (source == null)
            throw new ArgumentNullException("source");

        return source.Select(x => 
            EqualityComparer<T>.Default.Equals(x, oldValue) ? newValue : x);
    }

    public static IEnumerable<TSource> DistinctBy<TSource, TKey>(
        this IEnumerable<TSource> source,
        Func<TSource, TKey> keySelector)
    {
        HashSet<TKey> seenKeys = new HashSet<TKey>();
        foreach (TSource element in source)
        {
            if (seenKeys.Add(keySelector(element)))
            {
                yield return element;
            }
        }
    }
}
