﻿namespace Orbital7.Extensions;

public class DefaultValueDictionary<TKey, TValue> :
    Dictionary<TKey, TValue>
    where TKey : notnull
{
    public new TValue? this[TKey key]
    {
        get
        {
            if (TryGetValue(key, out var value))
            {
                return value;
            }
            return default;
        }
        set
        {
            if (value != null)
            {
                base[key] = value;
            }
        }
    }

    public DefaultValueDictionary() :
        base()
    {

    }

    public DefaultValueDictionary(
        IEqualityComparer<TKey> comparer) :
        base(comparer)
    {

    }

    public DefaultValueDictionary(
        IDictionary<TKey, TValue> dictionary) :
        base(dictionary)
    {

    }
}
