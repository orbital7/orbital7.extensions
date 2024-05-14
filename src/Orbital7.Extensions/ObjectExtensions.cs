using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;

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

    public static List<PropertyValue> GetPropertyValues(
        this object model)
    {
        var type = model.GetType();
        PropertyInfo[] properties = type.GetProperties();

        return properties
            .Select(x => new PropertyValue(x.Name, x.GetDisplayName(), x.GetValue(model)))
            .OrderBy(x => x.DisplayName)
            .ToList();
    }

    public static List<PropertyValue> ToPropertyValuesList<TEntity>(
        this TEntity entity,
        params Expression<Func<TEntity, object>>[] properties)
        where TEntity : class
    {
        var list = new List<PropertyValue>();

        foreach (var property in properties)
        {
            var memberInfo = property.Body.GetPropertyInformation();
            list.Add(new PropertyValue()
            {
                Name = memberInfo.Name,
                DisplayName = memberInfo.GetDisplayName(),
                Value = property.Compile().Invoke(entity),
            });
        }

        return list;
    }

    public static List<PropertyValue> ToPropertyValuesList<TEntity>(
        this TEntity entity,
        params (Expression<Func<TEntity, object>>, string)[] propertiesWithDisplayNameOverrides)
        where TEntity : class
    {
        var list = new List<PropertyValue>();

        foreach (var property in propertiesWithDisplayNameOverrides)
        {
            var memberInfo = property.Item1.Body.GetPropertyInformation();
            list.Add(new PropertyValue()
            {
                Name = property.Item1.Name,

                DisplayName = property.Item2.HasText() ?
                    property.Item2 :
                    memberInfo.GetDisplayName(),

                Value = property.Item1
                    .Compile()
                    .Invoke(entity),
            });
        }

        return list;
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
