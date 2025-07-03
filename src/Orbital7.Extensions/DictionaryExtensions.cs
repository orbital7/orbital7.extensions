namespace Orbital7.Extensions;

public static class DictionaryExtensions
{
    public static IDictionary<TKey, TValue> AddSingle<TKey, TValue>(
        this IDictionary<TKey, TValue> dictionary, 
        TKey key, 
        TValue value)
    {
        dictionary.Add(key, value);

        return dictionary;
    }

    public static IDictionary<TKey, string> AddSingleOrDefault<TKey>(
        this IDictionary<TKey, string> dictionary,
        TKey key,
        string? value)
    {
        dictionary.Add(key, value ?? string.Empty);

        return dictionary;
    }

    public static IDictionary<TKey, TValue> AddIfMissing<TKey, TValue>(
        this IDictionary<TKey, TValue> dictionary,
        TKey key,
        TValue value)
    {
        if (!dictionary.ContainsKey(key))
        {
            dictionary.Add(key, value);
        }

        return dictionary;
    }

    public static IDictionary<TKey, string> AddOrAppendToExisting<TKey>(
        this IDictionary<TKey, string> dictionary,
        TKey key,
        string value,
        string append = " ")
    {
        if (!dictionary.ContainsKey(key))
        {
            dictionary.Add(key, value);
        }
        else
        {
            dictionary[key] = dictionary[key] + append + value;
        }

        return dictionary;
    }

    public static IDictionary<TKey, TValue> AddRange<TKey, TValue>(
        this IDictionary<TKey, TValue> dictionary,
        IDictionary<TKey, TValue> dictionaryToAdd)
    {
        if (dictionaryToAdd != null && dictionaryToAdd.Any())
        {
            foreach (var item in dictionaryToAdd)
            {
                dictionary.Add(item.Key, item.Value);
            }
        }

        return dictionary;
    }

    public static IDictionary<TKey, TValue> ShallowClone<TKey, TValue>(
        this IDictionary<TKey, TValue> dictionary)
        where TKey : notnull
    {
        var clonedDictionary = new Dictionary<TKey, TValue>();
        clonedDictionary.AddRange(dictionary);

        return clonedDictionary;
    }

    public static TValue? GetValueOrDefault<TKey, TValue>(
        this IDictionary<TKey, TValue> dictionary,
        TKey key)
        where TKey : notnull
    {
        return dictionary.TryGetValue(key, out TValue? value) ? 
            value : 
            default;
    }
}
