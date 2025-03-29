using System.Collections;

namespace System;

public static class ObjectExtensions
{
    public static T CloneIgnoringReferenceProperties<T>(
        this T obj)
        where T : class, new()
    {
        var clone = new T();

        if (clone is IList list)
        {
            foreach (var item in (IList)obj)
            {
                list.Add(item.CloneIgnoringReferenceProperties());
            }
        }
        else
        {
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
        }

        return clone;
    }
}