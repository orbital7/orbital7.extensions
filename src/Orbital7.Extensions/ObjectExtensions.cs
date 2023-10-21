using MassTransit;
using System.ComponentModel.DataAnnotations;

namespace System;

public static class ObjectExtensions
{
    public static ObjectValidationResult Validate(
        this object model)
    {
        var results = new List<ValidationResult>();
        var context = new ValidationContext(model);

        var isValid = Validator.TryValidateObject(model, context, results, true);

        return new ObjectValidationResult()
        {
            IsValid = isValid,
            Results = results,
        };
    }

    public static void EnsureIsValid(
        this object model)
    {
        var result = model.Validate();
        if (!result.IsValid)
            throw new ValidationException(result.ToString());
    }

    public static List<SerializableTuple<string, object>> GetPropertyValues(
        this object model)
    {
        var type = model.GetType();
        PropertyInfo[] properties = type.GetProperties();

        return properties
            .Select(x => new SerializableTuple<string, object>()
            {
                Item1 = x.Name,
                Item2 = x.GetValue(model)
            })
            .OrderBy(x => x.Item1)
            .ToList();
    }

    public static T CloneIgnoringReferenceProperties<T>(
        this T model)
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
                    property.GetValue(model));
            }
        }

        return clone;
    }

    public static T CloneAsNewEntityIgnoringReferenceProperties<T>(
        this T model)
        where T : class, IEntity, new()
    {
        var entity = model.CloneIgnoringReferenceProperties();
        entity.Id = NewId.NextSequentialGuid();
        entity.CreatedDateTimeUtc = DateTime.UtcNow;
        entity.LastModifiedDateTimeUtc = model.CreatedDateTimeUtc;

        return entity;
    }
}
