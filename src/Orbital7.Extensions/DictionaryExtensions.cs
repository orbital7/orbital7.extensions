namespace Orbital7.Extensions;

public static class DictionaryExtensions
{
    public static IDictionary<T1, T2> AddSingle<T1, T2>(
        this IDictionary<T1, T2> dictionary, 
        T1 key, 
        T2 value)
    {
        dictionary.Add(key, value);

        return dictionary;
    }


    public static IDictionary<T1, T2> AddRange<T1, T2>(
        this IDictionary<T1, T2> dictionary,
        IDictionary<T1, T2> dictionaryToAdd)
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

    public static IDictionary<T1, T2> ShallowClone<T1, T2>(
        this IDictionary<T1, T2> dictionary)
    {
        var clonedDictionary = new Dictionary<T1, T2>();
        clonedDictionary.AddRange(dictionary);

        return clonedDictionary;
    }

    public static T2 TryGetValue<T1, T2>(
        this IDictionary<T1, T2> dictionary,
        T1 key)
    {
        return dictionary.TryGetValue(key, out T2 value) ? value : default;
    }
}
