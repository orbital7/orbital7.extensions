using System.Collections.Concurrent;
using System.Runtime.InteropServices;

namespace Orbital7.Extensions;

public static class ObjectExtensions
{
    private static readonly ConcurrentDictionary<Type, PropertyInfo[]> s_propertiesCache
        = new();

    private static readonly ConcurrentDictionary<Type, Func<int, object>> s_collectionFactoryCache
        = new();

    public static TOutput CloneIgnoringReferenceProperties<TInput, TOutput>(
        this TInput obj,
        IDictionary<string, object?>? overrides = null)
        where TInput : class
        where TOutput : class
    {
        var stringType = typeof(string);
        var inputType = typeof(TInput);
        var outputType = typeof(TOutput);

        // Cache the properties on the basis of type.
        var inputProperties = s_propertiesCache.GetOrAdd(inputType, t =>
            t.GetProperties(BindingFlags.Public | BindingFlags.Instance));
        var outputProperties = s_propertiesCache.GetOrAdd(outputType, t =>
            t.GetProperties(BindingFlags.Public | BindingFlags.Instance));

        // Gather clonable values from the source object (non-class or string).
        // Use case-insensitive keys to ease matching to ctor parameter names.
        var propValues = new Dictionary<string, object?>(StringComparer.OrdinalIgnoreCase);
        foreach (var property in inputProperties)
        {
            if (property.CanRead &&
                (!property.PropertyType.IsClass || property.PropertyType == stringType))
            {
                propValues[property.Name] = property.GetValue(obj);
            }
        }

        // Apply overrides (overrides take precedence)
        if (overrides != null)
        {
            foreach (var kv in overrides)
                propValues[kv.Key] = kv.Value;
        }

        // Create the clone and set the properties.
        var clone = Activator.CreateInstance<TOutput>();
        foreach (var property in outputProperties)
        {
            // If the property is both readable and writable and
            // the property is either not a class property or a string,
            // set it.
            if (property.CanRead &&
                property.CanWrite &&
                (!property.PropertyType.IsClass ||
                 property.PropertyType == stringType))
            {
                property.SetValue(
                    clone,
                    propValues[property.Name]);
            }
        }

        return clone;
    }

    public static T CloneIgnoringReferenceProperties<T>(
        this T obj,
        IDictionary<string, object?>? overrides = null)
        where T : class
    {
        return obj.CloneIgnoringReferenceProperties<T, T>(overrides);
    }

    public static TOutputList CloneIgnoringReferenceProperties<TInput, TInputList, TOutput, TOutputList>(
        this TInputList list)
        where TInput : class
        where TInputList : ICollection<TInput>
        where TOutput : class
        where TOutputList : IList<TOutput>
    {
        // Get or build factory.
        var factory = s_collectionFactoryCache.GetOrAdd(typeof(TOutputList), t =>
        {
            if (t.IsArray)
            {
                return (Func<int, object>)(c => Array.CreateInstance(typeof(TOutput), c));
            }

            var ctorWithInt = t.GetConstructor(new[] { typeof(int) });
            if (ctorWithInt != null)
                return c => Activator.CreateInstance(t, c)!;

            var paramless = t.GetConstructor(Type.EmptyTypes);
            if (paramless != null)
                return c => Activator.CreateInstance(t)!;

            throw new InvalidOperationException($"Type {t.FullName} must have a public .ctor(int) or parameterless .ctor, or provide a factory.");
        });

        var clone = factory(list.Count);

        // If array, fill by index
        if (clone is TOutput[] arr)
        {
            int i = 0;
            foreach (var item in list)
                arr[i++] = item.CloneIgnoringReferenceProperties<TInput, TOutput>();
            return (TOutputList)(object)arr;
        }

        // Otherwise treat as ICollection<TOutput>
        var targetCollection = (ICollection<TOutput>)clone;
        foreach (var item in list)
            targetCollection.Add(item.CloneIgnoringReferenceProperties<TInput, TOutput>());

        return (TOutputList)targetCollection;
    }

    public static List<T> CloneIgnoringReferenceProperties<T>(
        this List<T> list)
        where T : class
    {
        return list.CloneIgnoringReferenceProperties<T, List<T>, T, List<T>>();
    }

    public static T[] CloneIgnoringReferenceProperties<T>(
        this T[] array)
        where T : class, new()
    {
        var clone = new T[array.Length];

        for (int i = 0; i < array.Length; i++)
        {
            clone[i] = array[i].CloneIgnoringReferenceProperties();
        }

        return clone;
    }
}