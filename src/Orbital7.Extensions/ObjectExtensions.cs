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

    public static List<NamedValue<object>> GetPropertyValues(
        this object model)
    {
        var type = model.GetType();
        PropertyInfo[] properties = type.GetProperties();

        return properties
            .Select(x => new NamedValue<object>(x.Name, x.GetValue(model)))
            .OrderBy(x => x.Name)
            .ToList();
    }

    public static TEntity CloneIgnoringReferenceProperties<TEntity>(
        this TEntity model)
        where TEntity : class, new()
    {
        var clone = new TEntity();

        var stringPropertyType = typeof(string);
        var properties = typeof(TEntity).GetProperties();
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

    public static TEntity CloneAsNewEntityIgnoringReferenceProperties<TEntity>(
        this TEntity model)
        where TEntity : EntityGuidKeyedBase, new()
    {
        return ExecuteCloneAsNewEntityIgnoringReferenceProperties(
            model,
            GuidFactory.NextSequential());
    }

    public static TEntity CloneAsNewEntityIgnoringReferenceProperties<TEntity>(
        this TEntity model,
        long newId = 0L)
        where TEntity : EntityLongKeyedBase, new()
    {
        return ExecuteCloneAsNewEntityIgnoringReferenceProperties(
            model,
            newId);
    }

    private static TEntity ExecuteCloneAsNewEntityIgnoringReferenceProperties<TEntity, TKey>(
        TEntity model,
        TKey id)
        where TEntity : class, IEntity<TKey>, new()
        where TKey : IEquatable<TKey>
    {
        var entity = model.CloneIgnoringReferenceProperties();
        entity.Id = id;
        entity.CreatedDateTimeUtc = DateTime.UtcNow;
        entity.LastModifiedDateTimeUtc = model.CreatedDateTimeUtc;

        return entity;
    }
}
