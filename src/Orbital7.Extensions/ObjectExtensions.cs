namespace Orbital7.Extensions;

public static class ObjectExtensions
{
    public static T CloneIgnoringReferenceProperties<T>(
        this T obj)
        where T : class, new()
    {
        var clone = new T();
        var stringPropertyType = typeof(string);
        var properties = typeof(T).GetProperties();

        foreach (var property in properties)
        {
            // If the property is both readable and writable and
            // the property is either not a class property or a string,
            // set it.
            if (property.CanRead &&
                property.CanWrite &&
                (!property.PropertyType.IsClass ||
                 property.PropertyType == stringPropertyType))
            {
                property.SetValue(
                    clone,
                    property.GetValue(obj));
            }
        }

        return clone;
    }

    public static List<T> CloneIgnoringReferenceProperties<T>(
        this List<T> list)
        where T : class, new()
    {
        var clone = new List<T>(list.Count);

        foreach (var item in list)
        {
            clone.Add(item.CloneIgnoringReferenceProperties());
        }

        return clone;
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