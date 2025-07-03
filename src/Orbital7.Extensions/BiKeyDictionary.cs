namespace Orbital7.Extensions;

public class BiKeyDictionary<TKey1, TKey2, TValue>
    where TKey1 : notnull
    where TKey2 : notnull
    where TValue : notnull
{
    private readonly Dictionary<TKey1, TValue> _byKey1 = new();
    private readonly Dictionary<TKey2, TValue> _byKey2 = new();

    public ICollection<TValue> Values => _byKey2.Values;

    public BiKeyDictionary()
    {

    }

    public BiKeyDictionary(
        IEnumerable<(TKey1, TKey2, TValue)> items)
    {
        foreach (var item in items)
        {
            Add(
                item.Item1, 
                item.Item2, 
                item.Item3);
        }
    }

    public void Add(
        TKey1 key1, 
        TKey2 key2, 
        TValue value)
    {
        _byKey1.Add(key1, value);
        _byKey2.Add(key2, value);
    }

    public TValue GetValueByKey1(
        TKey1 key1)
    {
        return _byKey1[key1];
    }

    public TValue GetValueByKey2(
        TKey2 key2)
    {
        return _byKey2[key2];
    }


    public TValue? GetValueOrDefaultByKey1(
        TKey1 key1)
    {
        if (_byKey1.TryGetValue(key1, out var value))
        {
            return value;
        }

        return default;
    }

    public TValue? GetValueOrDefaultByKey2(
        TKey2 key2)
    {
        if (_byKey2.TryGetValue(key2, out var value))
        {
            return value;
        }

        return default;
    }

    public bool TryGetValueByKey1(
        TKey1 key1,
        out TValue? value)
    {
        return _byKey1.TryGetValue(key1, out value);
    }

    public bool TryGetValueByKey2(
        TKey2 key2,
        out TValue? value)
    {
        return _byKey2.TryGetValue(key2, out value);
    }

    public bool ContainsKey1(
        TKey1 key1)
    {
        return _byKey1.ContainsKey(key1);
    }

    public bool ContainsKey2(
        TKey2 key2)
    {
        return _byKey2.ContainsKey(key2);
    }

    public void Set(
        TKey1 key1, 
        TKey2 key2, 
        TValue value)
    {
        _byKey1[key1] = value;
        _byKey2[key2] = value;
    }

    public void Clear() 
    {
        _byKey1.Clear();
        _byKey2.Clear();
    }
}
