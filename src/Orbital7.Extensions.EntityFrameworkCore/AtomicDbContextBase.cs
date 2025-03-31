namespace Orbital7.Extensions.EntityFrameworkCore;

public abstract class AtomicDbContextBase :
    DbContext
{
    public bool ClearChangeTrackerOnSave { get; set; } = true;

    public virtual bool IsReadOnly => false;

    protected AtomicDbContextBase()
    {

    }

    protected AtomicDbContextBase(
        DbContextOptions options) :
        base(options)
    {
        if (this.Database.IsRelational())
        {
            this.Database.SetCommandTimeout(3600);
        }
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
