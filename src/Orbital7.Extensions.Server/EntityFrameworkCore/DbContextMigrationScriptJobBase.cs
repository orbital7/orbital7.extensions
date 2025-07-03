using Orbital7.Extensions.ScriptJobs;

namespace Orbital7.Extensions.EntityFrameworkCore;

public abstract class DbContextMigrationScriptJobBase<TDbContext> :
    ScriptJobBase
    where TDbContext : DbContext
{
    protected IServiceProvider ServiceProvider { get; private set; }

    protected IServiceProviderInitializer? Initializer { get; private set; }

    protected string? InitializingParam =>
        Initializer != null ? " and *INITIALIZE*" : null;

    protected DbContextMigrationScriptJobBase(
        IServiceProvider serviceProvider,
        IServiceProviderInitializer? initializer = null)
    {
        ServiceProvider = serviceProvider;
        Initializer = initializer;
    }

    protected abstract bool CanInitialize();

    protected abstract bool DeleteOnInitialize();

    public async override Task ExecuteAsync()
    {
        if (Initializer != null)
        {
            if (DeleteOnInitialize())
            {
                await DbContextMigrationHelper<TDbContext>.DeleteMigrateAndInitializeAsync(
                    ServiceProvider,
                    Initializer,
                    CanInitialize);
            }
            else
            {
                await DbContextMigrationHelper<TDbContext>.MigrateAndInitializeAsync(
                    ServiceProvider,
                    Initializer,
                    CanInitialize);
            }
            
        }
        else
        {
            await DbContextMigrationHelper<TDbContext>.MigrateAsync(
                ServiceProvider);
        }
    }
}