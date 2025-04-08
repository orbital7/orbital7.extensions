namespace Orbital7.Extensions.Data;

public static class EntityExtensions
{
    public static TEntity CloneAsNewEntityIgnoringReferenceProperties<TEntity>(
        this TEntity entity)
        where TEntity : EntityGuidKeyedBase, new()
    {
        return ExecuteCloneAsNewEntityIgnoringReferenceProperties(
            entity,
            GuidFactory.NextSequential());
    }

    public static TEntity CloneAsNewEntityIgnoringReferenceProperties<TEntity>(
        this TEntity entity,
        long newId = 0L)
        where TEntity : EntityLongKeyedBase, new()
    {
        return ExecuteCloneAsNewEntityIgnoringReferenceProperties(
            entity,
            newId);
    }

    public static List<PropertyValueChange> CalculateChanges<T>(
        this T oldEntity,
        T newEntity,
        List<string>? entityPropertyNamesToIgnore = null,
        List<(string, string)>? entityPropertyDisplayNameOverrides = null,
        List<(string, Func<object, string>)>? entityPropertyDisplayValueFormatters = null)
        where T : class, IEntity, new()
    {
        // Get the property values for both the old and new entities, ignoring
        // reference properties.
        var oldEntityProperties = ReflectionHelper.GetPropertyValues(
            oldEntity.CloneIgnoringReferenceProperties());
        var newEntityProperties = ReflectionHelper.GetPropertyValues(
            newEntity.CloneIgnoringReferenceProperties());

        var changes = new List<PropertyValueChange>();
        foreach (var property in oldEntityProperties)
        {
            // Ignore baseline entity properties.
            if (property.Name.HasText() &&
                property.Name != "Id" && //nameof(IEntity.Id) &&
                property.Name != nameof(IEntity.CreatedDateTimeUtc) &&
                property.Name != nameof(IEntity.LastModifiedDateTimeUtc) &&
                (entityPropertyNamesToIgnore == null ||
                 !entityPropertyNamesToIgnore.Contains(property.Name)))
            {
                var newPropertyValue = newEntityProperties
                    .Where(x => x.Name == property.Name)
                    .Select(x => x.Value)
                    .Single();

                // Determine if property value has changed.
                if (property.Value == null && newPropertyValue != null ||
                    property.Value != null && newPropertyValue == null ||
                    property.Value != null && newPropertyValue != null &&
                     !property.Value.Equals(newPropertyValue))
                {
                    var displayNameOverride = entityPropertyDisplayNameOverrides?
                        .Where(x => x.Item1 == property.Name)
                        .Select(x => x.Item2)
                        .SingleOrDefault();

                    var displayValueFormatter = entityPropertyDisplayValueFormatters?
                        .Where(x => x.Item1 == property.Name)
                        .Select(x => x.Item2)
                        .SingleOrDefault();

                    changes.Add(new PropertyValueChange()
                    {
                        PropertyName = property.Name,
                        PropertyDisplayName = displayNameOverride ?? property.DisplayName,
                        OldValue = displayValueFormatter != null && property.Value != null ?
                            displayValueFormatter.Invoke(property.Value) :
                            property.Value?.ToString(),
                        NewValue = displayValueFormatter != null && newPropertyValue != null ?
                            displayValueFormatter.Invoke(newPropertyValue) :
                            newPropertyValue?.ToString(),
                    });
                }
            }
        }

        return changes;
    }

    private static TEntity ExecuteCloneAsNewEntityIgnoringReferenceProperties<TEntity, TKey>(
        TEntity entity,
        TKey id)
        where TEntity : class, IEntity<TKey>, new()
        where TKey : IEquatable<TKey>
    {
        var clonedEntity = entity.CloneIgnoringReferenceProperties();
        clonedEntity.Id = id;
        clonedEntity.CreatedDateTimeUtc = DateTime.UtcNow;
        clonedEntity.LastModifiedDateTimeUtc = entity.CreatedDateTimeUtc;

        return clonedEntity;
    }
}
