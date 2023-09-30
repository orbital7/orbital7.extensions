using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System.Linq.Expressions;

namespace Microsoft.EntityFrameworkCore;

public static class Extensions
{
    public static ModelConfigurationBuilder SetDefaults(
        this ModelConfigurationBuilder builder)
    {
        builder.Properties<DateOnly>()
            .HaveConversion<DateOnlyConverter>()
            .HaveColumnType("date");

        builder.Properties<TimeOnly>()
            .HaveConversion<TimeOnlyConverter>()
            .HaveColumnType("time");

        return builder;
    }

    public static ModelBuilder SetDefaults(
        this ModelBuilder modelBuilder)
    {
        // Set default decimal precision.
        var decimalProperties = modelBuilder.GetPropertiesForType(typeof(decimal), typeof(decimal?));
        foreach (var property in decimalProperties)
        {
            property.SetPrecision(18);
            property.SetScale(2);
        }

        // Set property comparer for DateOnly.
        var dateOnlyProperties = modelBuilder.GetPropertiesForType(typeof(DateOnly), typeof(DateOnly?));
        foreach (var property in dateOnlyProperties)
        {
            property.SetValueComparer(new DateOnlyComparer());
        }

        // Set property comparer for TimeOnly.
        var timeOnlyProperties = modelBuilder.GetPropertiesForType(typeof(TimeOnly), typeof(TimeOnly?));
        foreach (var property in timeOnlyProperties)
        {
            property.SetValueComparer(new TimeOnlyComparer());
        }

        return modelBuilder;
    }

    public static IEnumerable<IMutableProperty> GetPropertiesForType(
        this ModelBuilder modelBuilder,
        Type type,
        Type nullableType)
    {
        var properties = modelBuilder.Model.GetEntityTypes()
            .SelectMany(t => t.GetProperties())
            .Where(p => p.ClrType == type || p.ClrType == nullableType);

        return properties;
    }

    public static TEntity UpdateEntity<TEntity>(
        this DbContext context,
        TEntity entity,
        bool ensureIsValid = true)
        where TEntity : class, IEntity
    {
        if (ensureIsValid)
            entity.EnsureIsValid();

        context.Entry(entity).State = EntityState.Modified;
        entity.LastModifiedDateTimeUtc = DateTime.UtcNow;
        return entity;
    }

    public static TEntity UpdateEntityProperty<TEntity, TProperty>(
        this DbContext context,
        TEntity entity,
        Expression<Func<TEntity, TProperty>> propertyExpression)
        where TEntity : class, IEntity
    {
        var entry = context.Entry(entity);
        entry.State = EntityState.Unchanged;

        entry.Property(propertyExpression).IsModified = true;

        entity.LastModifiedDateTimeUtc = DateTime.UtcNow;
        entry.Property(x => x.LastModifiedDateTimeUtc).IsModified = true;

        return entity;
    }

    public static TEntity UpdateEntityProperties<TEntity>(
        this DbContext context,
        TEntity entity,
        params string[] propertyNames)
        where TEntity : class, IEntity
    {
        var entry = context.Entry(entity);
        entry.State = EntityState.Unchanged;

        foreach (var property in propertyNames)
            entry.Property(property).IsModified = true;

        entity.LastModifiedDateTimeUtc = DateTime.UtcNow;
        entry.Property(x => x.LastModifiedDateTimeUtc).IsModified = true;

        return entity;
    }

    public static TEntity DeleteEntity<TEntity>(
        this DbContext context,
        TEntity entity)
        where TEntity : class, IEntity
    {
        var entry = context.Entry(entity);
        entry.State = EntityState.Deleted;
        return entry.Entity;
    }

    public static TEntity AddEntity<TEntity>(
        this DbSet<TEntity> entitySet,
        TEntity entity)
        where TEntity : class, IEntity
    {
        entitySet.Add(entity);
        return entity;
    }

    public static async Task<TEntity> GetEntityAsync<TEntity>(
        this DbSet<TEntity> entitySet,
        Guid id,
        params string[] includePaths)
        where TEntity : class, IEntity
    {
        return await ExecuteGetEntityAsync(entitySet, id, true, null, includePaths);
    }

    public static async Task<TEntity> GetEntityAsync<TEntity>(
        this DbSet<TEntity> entitySet,
        Guid id,
        CancellationToken cancellationToken,
        params string[] includePaths)
        where TEntity : class, IEntity
    {
        return await ExecuteGetEntityAsync(entitySet, id, true, cancellationToken, includePaths);
    }

    public static async Task<TEntity> GetEntityWithTrackingAsync<TEntity>(
        this DbSet<TEntity> entitySet,
        Guid id,
        params string[] includePaths)
        where TEntity : class, IEntity
    {
        return await ExecuteGetEntityAsync(entitySet, id, false, null, includePaths);
    }

    public static async Task<TEntity> GetEntityWithTrackingAsync<TEntity>(
        this DbSet<TEntity> entitySet,
        Guid id,
        CancellationToken cancellationToken,
        params string[] includePaths)
        where TEntity : class, IEntity
    {
        return await ExecuteGetEntityAsync(entitySet, id, false, cancellationToken, includePaths);
    }

    public static async Task<TEntity> GetEntityAsync<TEntity>(
        this DbSet<TEntity> entitySet,
        Expression<Func<TEntity, bool>> selectQuery,
        params string[] includePaths)
        where TEntity : class, IEntity
    {
        return await ExecuteGetEntityAsync(entitySet, selectQuery, true, null, includePaths);
    }

    public static async Task<TEntity> GetEntityAsync<TEntity>(
        this DbSet<TEntity> entitySet,
        Expression<Func<TEntity, bool>> selectQuery,
        CancellationToken cancellationToken,
        params string[] includePaths)
        where TEntity : class, IEntity
    {
        return await ExecuteGetEntityAsync(entitySet, selectQuery, true, cancellationToken, includePaths);
    }

    public static async Task<TEntity> GetEntityWithTrackingAsync<TEntity>(
        this DbSet<TEntity> entitySet,
        Expression<Func<TEntity, bool>> selectQuery,
        params string[] includePaths)
        where TEntity : class, IEntity
    {
        return await ExecuteGetEntityAsync(entitySet, selectQuery, false, null, includePaths);
    }

    public static async Task<TEntity> GetEntityWithTrackingAsync<TEntity>(
        this DbSet<TEntity> entitySet,
        Expression<Func<TEntity, bool>> selectQuery,
        CancellationToken cancellationToken,
        params string[] includePaths)
        where TEntity : class, IEntity
    {
        return await ExecuteGetEntityAsync(entitySet, selectQuery, false, cancellationToken, includePaths);
    }

    public static async Task<List<TEntity>> GatherEntitiesAsync<TEntity>(
        this DbSet<TEntity> entitySet,
        Expression<Func<TEntity, bool>> selectQuery,
        params string[] includePaths)
        where TEntity : class, IEntity
    {
        return await ExecuteGatherEntitiesAsync(entitySet, selectQuery, true, null, includePaths);
    }

    public static async Task<List<TEntity>> GatherEntitiesAsync<TEntity>(
        this DbSet<TEntity> entitySet,
        Expression<Func<TEntity, bool>> selectQuery,
        CancellationToken cancellationToken,
        params string[] includePaths)
        where TEntity : class, IEntity
    {
        return await ExecuteGatherEntitiesAsync(entitySet, selectQuery, true, cancellationToken, includePaths);
    }

    public static async Task<List<TEntity>> GatherEntitiesWithTrackingAsync<TEntity>(
        this DbSet<TEntity> entitySet,
        Expression<Func<TEntity, bool>> selectQuery,
        params string[] includePaths)
        where TEntity : class, IEntity
    {
        return await ExecuteGatherEntitiesAsync(entitySet, selectQuery, false, null, includePaths);
    }

    public static async Task<List<TEntity>> GatherEntitiesWithTrackingAsync<TEntity>(
        this DbSet<TEntity> entitySet,
        Expression<Func<TEntity, bool>> selectQuery,
        CancellationToken cancellationToken,
        params string[] includePaths)
        where TEntity : class, IEntity
    {
        return await ExecuteGatherEntitiesAsync(entitySet, selectQuery, false, cancellationToken, includePaths);
    }

    private static async Task<TEntity> ExecuteGetEntityAsync<TEntity>(
        DbSet<TEntity> entitySet,
        Guid id,
        bool untracked,
        CancellationToken? cancellationToken,
        params string[] includePaths)
        where TEntity : class, IEntity
    {
        var query = entitySet.Where(x => x.Id == id);
        query = CompleteEntityQuery(query, untracked, includePaths);

        if (cancellationToken.HasValue)
        {
            return await query.SingleOrDefaultAsync(cancellationToken.Value);
        }
        else
        {
            return await query.SingleOrDefaultAsync();
        }
    }

    private static async Task<TEntity> ExecuteGetEntityAsync<TEntity>(
        DbSet<TEntity> entitySet,
        Expression<Func<TEntity, bool>> selectQuery,
        bool untracked,
        CancellationToken? cancellationToken,
        params string[] includePaths)
        where TEntity : class, IEntity
    {
        var query = entitySet.Where(selectQuery);
        query = CompleteEntityQuery(query, untracked, includePaths);

        if (cancellationToken.HasValue)
        {
            return await query.FirstOrDefaultAsync(cancellationToken.Value);
        }
        else
        {
            return await query.FirstOrDefaultAsync();
        }
    }

    private static async Task<List<TEntity>> ExecuteGatherEntitiesAsync<TEntity>(
        DbSet<TEntity> entitySet,
        Expression<Func<TEntity, bool>> selectQuery,
        bool untracked,
        CancellationToken? cancellationToken,
        params string[] includePaths)
        where TEntity : class, IEntity
    {
        var query = entitySet.Where(selectQuery);
        query = CompleteEntityQuery(query, untracked, includePaths);

        if (cancellationToken != null)
        {
            return await query.ToListAsync(cancellationToken.Value);
        }
        else
        {
            return await query.ToListAsync();
        }
    }

    private static IQueryable<TEntity> CompleteEntityQuery<TEntity>(
        IQueryable<TEntity> query,
        bool untracked = true,
        params string[] includePaths)
        where TEntity : class, IEntity
    {
        foreach (var includePath in includePaths)
        {
            query = query.Include(includePath);
        }

        if (untracked)
        {
            query = query.AsNoTracking();
        }

        return query;
    }
}
