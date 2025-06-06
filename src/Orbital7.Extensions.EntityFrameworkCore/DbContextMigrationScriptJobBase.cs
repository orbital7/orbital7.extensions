using Orbital7.Extensions.ScriptJobs;

namespace Orbital7.Extensions.EntityFrameworkCore;

public abstract class DbContextMigrationScriptJobBase<TDbContext> :
    ScriptJobBase
    where TDbContext : DbContext
{
    protected IServiceProvider ServiceProvider { get; private set; }

    protected IServiceProviderInitializer? Initializer { get; private set; }

    protected string? InitializingParam =>
        this.Initializer != null ? " and *INITIALIZE*" : null;

    protected DbContextMigrationScriptJobBase(
        IServiceProvider serviceProvider,
        IServiceProviderInitializer? initializer = null)
    {
        this.ServiceProvider = serviceProvider;
        this.Initializer = initializer;
    }

    protected abstract bool CanInitialize();

    protected abstract bool DeleteOnInitialize();

    public async override Task ExecuteAsync()
    {
        if (this.Initializer != null)
        {
            if (this.DeleteOnInitialize())
            {
                await DbContextMigrationHelper<TDbContext>.DeleteMigrateAndInitializeAsync(
                    this.ServiceProvider,
                    this.Initializer,
                    this.CanInitialize);
            }
            else
            {
                await DbContextMigrationHelper<TDbContext>.MigrateAndInitializeAsync(
                    this.ServiceProvider,
                    this.Initializer,
                    this.CanInitialize);
            }
            
        }
        else
        {
            await DbContextMigrationHelper<TDbContext>.MigrateAsync(
                this.ServiceProvider);
        }
    }
}