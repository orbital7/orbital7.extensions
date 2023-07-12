namespace Microsoft.EntityFrameworkCore;

public interface IEntityService<TDbContext, TEntity>
    where TDbContext : DbContext
    where TEntity : class, IEntity
{
    TDbContext Context { get; }

    DbSet<TEntity> EntitySet { get; }

    Task<TEntity> GetAsync(
        Guid id,
        bool untracked = true,
        params string[] includePaths);
}