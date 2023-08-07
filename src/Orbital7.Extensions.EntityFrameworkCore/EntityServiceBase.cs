using System.Security.Principal;

namespace Microsoft.EntityFrameworkCore;

public abstract class EntityServiceBase<TDbContext, TEntity> :
    IEntityService<TDbContext, TEntity>
    where TDbContext : DbContext
    where TEntity : class, IEntity
{
    public TDbContext Context { get; private set; }

    public DbSet<TEntity> EntitySet { get; private set; }

    protected EntityServiceBase(
        TDbContext context)
    {
        this.Context = context;
        this.EntitySet = context.Set<TEntity>();
    }

    public async Task<TEntity> GetAsync(
        Guid id,
        bool untracked = true,
        params string[] includePaths)
    {
        var query = this.EntitySet
            .Where(x => x.Id == id);
        query = await ConfigureGetQueryFilterAsync(query);

        foreach (var includePath in includePaths)
            query = query.Include(includePath);

        if (untracked)
            query = query.AsNoTracking();

        return await query.FirstOrDefaultAsync();
    }

    protected virtual async Task<IQueryable<TEntity>> ConfigureGetQueryFilterAsync(
        IQueryable<TEntity> query)
    {
        return await Task.FromResult(query);
    }

    protected TOtherEntity AddEntity<TOtherEntity>(
        TOtherEntity entity,
        bool ensureIsValid = true)
        where TOtherEntity : class, IEntity
    {
        if (ensureIsValid)
            entity.EnsureIsValid();

        return this.Context.Set<TOtherEntity>().Add(entity).Entity;
    }

    protected TOtherEntity UpdateEntity<TOtherEntity>(
        TOtherEntity entity,
        bool ensureIsValid = true)
        where TOtherEntity : class, IEntity
    {
        if (ensureIsValid)
            entity.EnsureIsValid();

        this.Context.Entry(entity).State = EntityState.Modified;
        entity.LastModifiedDateTimeUtc = DateTime.UtcNow;
        return entity;
    }

    protected TOtherEntity UpdateEntityProperty<TOtherEntity, TProperty>(
        TOtherEntity entity,
        System.Linq.Expressions.Expression<Func<TOtherEntity, TProperty>> propertyExpression)
        where TOtherEntity : class, IEntity
    {
        var entry = this.Context.Entry(entity);
        entry.State = EntityState.Unchanged;

        entry.Property(propertyExpression).IsModified = true;

        entity.LastModifiedDateTimeUtc = DateTime.UtcNow;
        entry.Property(x => x.LastModifiedDateTimeUtc).IsModified = true;

        return entity;
    }

    protected TOtherEntity UpdateEntityProperties<TOtherEntity>(
        TOtherEntity entity,
        params string[] propertyNames)
        where TOtherEntity : class, IEntity
    {
        var entry = this.Context.Entry(entity);
        entry.State = EntityState.Unchanged;

        foreach (var property in propertyNames)
            entry.Property(property).IsModified = true;

        entity.LastModifiedDateTimeUtc = DateTime.UtcNow;
        entry.Property(x => x.LastModifiedDateTimeUtc).IsModified = true;

        return entity;
    }

    protected TOtherEntity DeleteEntity<TOtherEntity>(
        TOtherEntity entity)
        where TOtherEntity : class, IEntity
    {
        var entry = this.Context.Entry(entity);
        entry.State = EntityState.Deleted;
        return entry.Entity;
    }
}
