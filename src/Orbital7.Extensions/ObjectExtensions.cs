using System.ComponentModel.DataAnnotations;
using System.Reflection;

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
        this object objectInstance)
    {
        var type = objectInstance.GetType();
        PropertyInfo[] properties = type.GetProperties();

        return properties
            .Select(x => new SerializableTuple<string, object>()
            {
                Item1 = x.Name,
                Item2 = x.GetValue(objectInstance)
            })
            .OrderBy(x => x.Item1)
            .ToList();
    }
}
