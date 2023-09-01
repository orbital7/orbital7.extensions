using Microsoft.EntityFrameworkCore;

namespace Microsoft.AspNetCore.Identity.EntityFrameworkCore;

public abstract class AutoClearingIdentityDbContextBase<TUser, TRole> :
    IdentityDbContext<TUser, TRole, Guid>
    where TUser : IdentityUser<Guid>
    where TRole : IdentityRole<Guid>
{
    public bool ClearChangeTrackerOnSave { get; set; } = true;

    protected AutoClearingIdentityDbContextBase()
    {

    }

    protected AutoClearingIdentityDbContextBase(
        DbContextOptions options) : 
        base(options)
    {

    }

    public override int SaveChanges()
    {
        return HandlePostSave(base.SaveChanges());
    }

    public override int SaveChanges(
        bool acceptAllChangesOnSuccess)
    {
        return HandlePostSave(base.SaveChanges(acceptAllChangesOnSuccess));
    }

    public override async Task<int> SaveChangesAsync(
        bool acceptAllChangesOnSuccess,
        CancellationToken cancellationToken = default)
    {
        return HandlePostSave(await base.SaveChangesAsync(
            acceptAllChangesOnSuccess,
            cancellationToken));
    }

    public override async Task<int> SaveChangesAsync(
        CancellationToken cancellationToken = default)
    {
        return HandlePostSave(
            await base.SaveChangesAsync(cancellationToken));
    }

    protected override void ConfigureConventions(
        ModelConfigurationBuilder builder)
    {
        base.ConfigureConventions(builder);
        builder.SetDefaults();
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
}
