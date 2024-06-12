using System.Linq.Expressions;

namespace Microsoft.EntityFrameworkCore;

public static class Extensions
{
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

    public static TEntity AddEntity<TEntity>(
        this DbSet<TEntity> entitySet,
        TEntity entity,
        bool validate = true)
        where TEntity : class
    {
        if (validate)
        {
            entity.EnsureIsValid();
        }

        entitySet.Add(entity);
        return entity;
    }

    public static TEntity UpdateEntity<TEntity>(
        this DbSet<TEntity> entitySet,
        TEntity entity,
        bool validate = true)
        where TEntity : class, IEntity
    {
        if (validate)
        {
            entity.EnsureIsValid();
        }

        var entry = entitySet.Entry(entity);
        if (entry.State != EntityState.Added)
        {
            entry.State = EntityState.Modified;
            entity.LastModifiedDateTimeUtc = DateTime.UtcNow;
        }
        return entity;
    }

    public static TEntity UpdateEntityProperties<TEntity>(
        this DbSet<TEntity> entitySet,
        TEntity entity,
        params Expression<Func<TEntity, object>>[] updatedProperties)
        where TEntity : class, IEntity
    {
        var entry = entitySet.Entry(entity);
        entry.State = EntityState.Unchanged;

        foreach (var property in updatedProperties)
        {
            entry.Property(property).IsModified = true;
        }

        entity.LastModifiedDateTimeUtc = DateTime.UtcNow;
        entry.Property(x => x.LastModifiedDateTimeUtc).IsModified = true;

        return entity;
    }

    public static TEntity UpdateEntityProperties<TEntity, TKey>(
        this DbSet<TEntity> entitySet,
        TKey entityId,
        params (Expression<Func<TEntity, object>>, object)[] updatedPropertyValues)
        where TEntity : class, IEntity<TKey>, new()
        where TKey : IEquatable<TKey>
    {
        var entity = new TEntity()
        {
            Id = entityId,
        };

        var entry = entitySet.Entry(entity);
        entry.State = EntityState.Unchanged;

        foreach (var property in updatedPropertyValues)
        {
            var entryProperty = entry.Property(property.Item1);
            entryProperty.CurrentValue = property.Item2;
            entryProperty.IsModified = true;
        }

        entity.LastModifiedDateTimeUtc = DateTime.UtcNow;
        entry.Property(x => x.LastModifiedDateTimeUtc).IsModified = true;

        return entity;
    }

    public static TEntity DeleteEntity<TEntity>(
        this DbSet<TEntity> entitySet,
        TEntity entity)
        where TEntity : class
    {
        var entry = entitySet.Remove(entity);
        return entry.Entity;
    }

    public static async Task<TEntity> GetAsync<TEntity, TKey>(
        this DbSet<TEntity> entitySet,
        TKey id,
        params string[] includePaths)
        where TEntity : class, IEntity<TKey>
        where TKey : IEquatable<TKey>
    {
        return await ExecuteGetAsync(entitySet, id, true, null, includePaths);
    }

    public static async Task<TEntity> GetAsync<TEntity, TKey>(
        this DbSet<TEntity> entitySet,
        TKey id,
        CancellationToken cancellationToken,
        params string[] includePaths)
        where TEntity : class, IEntity<TKey>
        where TKey : IEquatable<TKey>
    {
        return await ExecuteGetAsync(entitySet, id, true, cancellationToken, includePaths);
    }

    public static async Task<TOutput> GetAsync<TEntity, TKey, TOutput>(
        this DbSet<TEntity> entitySet,
        TKey id,
        Expression<Func<TEntity, TOutput>> select,
        CancellationToken cancellationToken = default)
        where TEntity : class, IEntity<TKey>
        where TKey : IEquatable<TKey>
    {
        return await entitySet
            .Where(x => x.Id.Equals(id))
            .Select(select)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public static async Task<TEntity> GetWithTrackingAsync<TEntity, TKey>(
        this DbSet<TEntity> entitySet,
        TKey id,
        params string[] includePaths)
        where TEntity : class, IEntity<TKey>
        where TKey : IEquatable<TKey>
    {
        return await ExecuteGetAsync(entitySet, id, false, null, includePaths);
    }

    public static async Task<TEntity> GetWithTrackingAsync<TEntity, TKey>(
        this DbSet<TEntity> entitySet,
        TKey id,
        CancellationToken cancellationToken,
        params string[] includePaths)
        where TEntity : class, IEntity<TKey>
        where TKey : IEquatable<TKey>
    {
        return await ExecuteGetAsync(entitySet, id, false, cancellationToken, includePaths);
    }

    public static async Task<TEntity> GetAsync<TEntity>(
        this IQueryable<TEntity> query,
        params string[] includePaths)
        where TEntity : class
    {
        return await ExecuteGetAsync(query, true, null, includePaths);
    }

    public static async Task<TEntity> GetAsync<TEntity>(
        this IQueryable<TEntity> query,
        CancellationToken cancellationToken,
        params string[] includePaths)
        where TEntity : class
    {
        return await ExecuteGetAsync(query, true, cancellationToken, includePaths);
    }

    public static async Task<TEntity> GetWithTrackingAsync<TEntity>(
        this IQueryable<TEntity> query,
        params string[] includePaths)
        where TEntity : class
    {
        return await ExecuteGetAsync(query, false, null, includePaths);
    }

    public static async Task<TEntity> GetWithTrackingAsync<TEntity>(
        this IQueryable<TEntity> query,
        CancellationToken cancellationToken,
        params string[] includePaths)
        where TEntity : class
    {
        return await ExecuteGetAsync(query, false, cancellationToken, includePaths);
    }

    public static async Task<TOutput> GetAsync<TEntity, TOutput>(
        this IQueryable<TEntity> query,
        Expression<Func<TEntity, TOutput>> select,
        CancellationToken cancellationToken = default)
        where TEntity : class
    {
        return await query
            .Select(select)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public static async Task<List<TEntity>> GatherAsync<TEntity>(
        this IQueryable<TEntity> query,
        params string[] includePaths)
        where TEntity : class
    {
        return await ExecuteGatherAsync(query, true, null, includePaths);
    }

    public static async Task<List<TEntity>> GatherAsync<TEntity>(
        this IQueryable<TEntity> query,
        CancellationToken cancellationToken,
        params string[] includePaths)
        where TEntity : class
    {
        return await ExecuteGatherAsync(query, true, cancellationToken, includePaths);
    }

    public static async Task<List<TEntity>> GatherWithTrackingAsync<TEntity>(
        this IQueryable<TEntity> query,
        params string[] includePaths)
        where TEntity : class
    {
        return await ExecuteGatherAsync(query, false, null, includePaths);
    }

    public static async Task<List<TEntity>> GatherWithTrackingAsync<TEntity>(
        this IQueryable<TEntity> query,
        CancellationToken cancellationToken,
        params string[] includePaths)
        where TEntity : class
    {
        return await ExecuteGatherAsync(query, false, cancellationToken, includePaths);
    }

    public static async Task<List<TOutput>> GatherAsync<TEntity, TOutput>(
        this IQueryable<TEntity> query,
        Expression<Func<TEntity, TOutput>> select,
        CancellationToken cancellationToken = default)
        where TEntity : class
    {
        return await query
            .Select(select)
            .ToListAsync(cancellationToken);
    }

    private static async Task<TEntity> ExecuteGetAsync<TEntity, TKey>(
        DbSet<TEntity> entitySet,
        TKey id,
        bool untracked,
        CancellationToken? cancellationToken,
        params string[] includePaths)
        where TEntity : class, IEntity<TKey>
        where TKey : IEquatable<TKey>
    {
        var query = entitySet.Where(x => x.Id.Equals(id));
        return await ExecuteGetAsync(
            query,
            untracked,
            cancellationToken,
            includePaths);
    }

    private static async Task<TEntity> ExecuteGetAsync<TEntity>(
        IQueryable<TEntity> query,
        bool untracked,
        CancellationToken? cancellationToken,
        params string[] includePaths)
        where TEntity : class
    {
        query = CompleteQuery(query, untracked, includePaths);

        if (cancellationToken.HasValue)
        {
            return await query.FirstOrDefaultAsync(cancellationToken.Value);
        }
        else
        {
            return await query.FirstOrDefaultAsync();
        }
    }

    private static async Task<List<TEntity>> ExecuteGatherAsync<TEntity>(
        IQueryable<TEntity> query,
        bool untracked,
        CancellationToken? cancellationToken,
        params string[] includePaths)
        where TEntity : class
    {
        query = CompleteQuery(query, untracked, includePaths);

        if (cancellationToken != null)
        {
            return await query.ToListAsync(cancellationToken.Value);
        }
        else
        {
            return await query.ToListAsync();
        }
    }

    private static IQueryable<TEntity> CompleteQuery<TEntity>(
        IQueryable<TEntity> query,
        bool untracked = true,
        params string[] includePaths)
        where TEntity : class
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
