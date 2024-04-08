namespace Microsoft.AspNetCore.Identity.EntityFrameworkCore;

public abstract class AtomicIdentityDbContextBase<TUser, TRole, TKey> :
    IdentityDbContext<TUser, TRole, TKey>
    where TUser : IdentityUser<TKey>
    where TRole : IdentityRole<TKey>
    where TKey : IEquatable<TKey>
{
    public bool ClearChangeTrackerOnSave { get; set; } = true;

    public virtual bool IsReadOnly => false;

    protected AtomicIdentityDbContextBase()
    {

    }

    protected AtomicIdentityDbContextBase(
        DbContextOptions options) : 
        base(options)
    {

    }

    public override int SaveChanges()
    {
        ValidateSave();
        return HandlePostSave(base.SaveChanges());
    }

    public override int SaveChanges(
        bool acceptAllChangesOnSuccess)
    {
        ValidateSave();
        return HandlePostSave(base.SaveChanges(acceptAllChangesOnSuccess));
    }

    public override async Task<int> SaveChangesAsync(
        bool acceptAllChangesOnSuccess,
        CancellationToken cancellationToken = default)
    {
        ValidateSave();
        return HandlePostSave(await base.SaveChangesAsync(
            acceptAllChangesOnSuccess,
            cancellationToken));
    }

    public override async Task<int> SaveChangesAsync(
        CancellationToken cancellationToken = default)
    {
        ValidateSave();
        return HandlePostSave(
            await base.SaveChangesAsync(cancellationToken));
    }

    protected override void OnModelCreating(
        ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.SetDefaults();
    }

    private int HandlePostSave(
        int result)
    {
        if (this.ClearChangeTrackerOnSave)
        {
            base.ChangeTracker.Clear();
        }

        return result;
    }

    private void ValidateSave()
    {
        if (this.IsReadOnly)
        {
            throw new Exception("Saving is not permitted on a read-only context");
        }
    }
}
