using System.Buffers;
using System.Text.Json;

namespace Orbital7.Extensions;

/// <summary>
/// Immutable, value-based list for structural equality (order-sensitive).
/// Use this as a record property when you want element-wise equality.
/// </summary>
[JsonConverter(typeof(ValueListJsonConverterFactory))]
public sealed class ValueList<T> :
    IReadOnlyList<T>,
    IEquatable<ValueList<T>>
{
    private readonly T[] _items;

    public ValueList(
        IEnumerable<T>? items)
    {
        _items = items is null ? Array.Empty<T>() : items.ToArray();
    }

    public int Count => _items.Length;

    public T this[int index] => _items[index];

    public IEnumerator<T> GetEnumerator() => ((IEnumerable<T>)_items).GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => _items.GetEnumerator();

    public static ValueList<T> Empty { get; } = new ValueList<T>(Array.Empty<T>());

    public static ValueList<T> From(IEnumerable<T> items) => new ValueList<T>(items);

    public T[] ToArray() => (T[])_items.Clone();

    public List<T> ToList() => new List<T>(_items);

    public bool Equals(
        ValueList<T>? other)
    {
        if (ReferenceEquals(this, other))
            return true;

        if (other is null)
            return false;

        if (Count != other.Count)
            return false;

        var comparer = EqualityComparer<T>.Default;
        for (int i = 0; i < _items.Length; i++)
        {
            if (!comparer.Equals(_items[i], other._items[i]))
                return false;
        }

        return true;
    }

    public override bool Equals(object? obj) =>
        Equals(obj as ValueList<T>);

    public override int GetHashCode()
    {
        // Order-sensitive aggregate hash. Include count to avoid collisions
        // for sequences like [1,23] vs [12,3] in some combination scenarios.
        var h = new HashCode();
        h.Add(Count);
        var comparer = EqualityComparer<T>.Default;
        foreach (var item in _items)
        {
            h.Add(item, comparer);
        }
        return h.ToHashCode();
    }

    public static bool operator ==(ValueList<T>? left, ValueList<T>? right) =>
        EqualityComparer<ValueList<T>>.Default.Equals(left, right);

    public static bool operator !=(ValueList<T>? left, ValueList<T>? right) =>
        !(left == right);

    public override string ToString() =>
        $"ValueList[{string.Join(", ", _items.Select(x => x?.ToString()))}]";

    public static implicit operator ValueList<T>(T[] items) => new ValueList<T>(items);
    public static implicit operator ValueList<T>(List<T> items) => new ValueList<T>(items);
}

/// <summary>
/// JsonConverterFactory for ValueList&lt;T&gt; so System.Text.Json can read/write ValueList as JSON arrays.
/// The converter serializes the ValueList as an array and deserializes arrays into ValueList instances.
/// </summary>
internal sealed class ValueListJsonConverterFactory : 
    JsonConverterFactory
{
    public override bool CanConvert(Type typeToConvert) =>
        typeToConvert.IsGenericType &&
        typeToConvert.GetGenericTypeDefinition() == typeof(ValueList<>);

    public override JsonConverter? CreateConverter(
        Type typeToConvert, 
        JsonSerializerOptions options)
    {
        var elementType = typeToConvert.GetGenericArguments()[0];
        var converterType = typeof(ValueListJsonConverter<>).MakeGenericType(elementType);
        return (JsonConverter?)Activator.CreateInstance(converterType);
    }
}

internal sealed class ValueListJsonConverter<TElement> : 
    JsonConverter<ValueList<TElement>>
{
    public override ValueList<TElement>? Read(
        ref Utf8JsonReader reader, 
        Type typeToConvert, 
        JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.Null)
        {
            return null;
        }

        // Deserialize the JSON array into an array of elements, then construct ValueList from it.
        var elements = JsonSerializer.Deserialize<TElement[]>(ref reader, options);
        if (elements is null)
            return new ValueList<TElement>(Array.Empty<TElement>());

        return new ValueList<TElement>(elements);
    }

    public override void Write(
        Utf8JsonWriter writer, 
        ValueList<TElement>? value, 
        JsonSerializerOptions options)
    {
        if (value is null)
        {
            writer.WriteNullValue();
            return;
        }

        // Serialize as an array using the underlying elements.
        JsonSerializer.Serialize(writer, value.ToArray(), options);
    }
}