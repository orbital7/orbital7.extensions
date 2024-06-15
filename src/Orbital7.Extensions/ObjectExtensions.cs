using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;

namespace System;

public static class ObjectExtensions
{
    public static ObjectValidationResult Validate(
        this object model)
    {
        return ValidateRecur(model, null);
    }

    public static void EnsureIsValid(
        this object model)
    {
        var result = model.Validate();
        if (!result.IsValid)
            throw new ValidationException(result.ToString());
    }

    public static object GetPropertyValue(
        this object model, 
        string propertyName)
    {
        object objValue = string.Empty;

        var propertyInfo = model
            .GetType()
            .GetProperty(propertyName);

        if (propertyInfo != null)
        {
            objValue = propertyInfo.GetValue(model, null);
        }

        return objValue;
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

    public static string GetDisplayName<TEntity>(
        this TEntity entity,
        Expression<Func<TEntity, object>> property)
        where TEntity : class
    {
        var memberInfo = property.Body.GetPropertyInformation();
        return memberInfo.GetDisplayName();
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

    private static ObjectValidationResult ValidateRecur(
        object model,
        string memberNamePrefix)
    {
        // NOTE: This method currently fails to validate Structs.

        var results = new List<ValidationResult>();
        var context = new ValidationContext(model);

        // Validate the base model.
        var isValid = Validator.TryValidateObject(model, context, results, true);

        // Create the validation result and apply the membername prefix if there
        // is one.
        var validationResult = new ObjectValidationResult()
        {
            IsValid = isValid,
            Results = memberNamePrefix.HasText() ?
                results
                    .Select(x => new ValidationResult(
                        x.ErrorMessage,
                        x.MemberNames.Select(x => $"{memberNamePrefix}.{x}").ToArray()))
                    .ToList() :
                results,
        };

        // Get the properties for the model (exclude any 
        // properties that require parameters, like indexers).
        var properties = model
            .GetType()
            .GetProperties()
            .Where(x => x.GetIndexParameters().Length == 0);

        // Recursively validate any complex properties.
        var stringType = typeof(string);
        var enumerableType = typeof(Collections.IEnumerable);
        foreach (var property in properties)
        {
            if (property.PropertyType.IsClass &&
                property.PropertyType != stringType)
            {
                var propertyValue = property.GetValue(model);
                if (propertyValue != null)
                {
                    // If the property is enumerable, we want to recursively loop
                    // through and validate the collection items.
                    if (enumerableType.IsAssignableFrom(property.PropertyType))
                    {
                        int i = 0;
                        foreach (var item in propertyValue as Collections.IEnumerable)
                        {
                            var propertyName = $"{property.Name}[{i}]";

                            var itemValidationResult = ValidateRecur(
                                item,
                                memberNamePrefix.HasText() ?
                                    memberNamePrefix + "." + propertyName :
                                    propertyName);

                            validationResult.Append(itemValidationResult);
                        }
                    }
                    // Else recursively validate the property value entity.
                    else
                    {
                        var propertyValidationResult = ValidateRecur(
                            propertyValue,
                            memberNamePrefix.HasText() ?
                                memberNamePrefix + "." + property.Name :
                                property.Name);

                        validationResult.Append(propertyValidationResult);
                    }
                }
            }
        }

        return validationResult;
    }
}