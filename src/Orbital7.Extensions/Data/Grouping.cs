namespace Orbital7.Extensions.Data;

public class Grouping<TKey, TElement> : 
    IGrouping<TKey, TElement>
{
    private readonly TKey _key;
    private readonly List<TElement> _values;

    public Grouping(TKey key, List<TElement> values)
    {
        if (values == null)
            throw new ArgumentNullException("values");
        this._key = key;
        this._values = values;
    }

    public TKey Key
    {
        get { return _key; }
    }

    public void Add(TElement element)
    {
        _values.Add(element);
    }

    public IEnumerator<TElement> GetEnumerator()
    {
        return _values.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}
