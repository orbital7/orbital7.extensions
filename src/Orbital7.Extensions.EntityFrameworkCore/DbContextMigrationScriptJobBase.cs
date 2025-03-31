using Orbital7.Extensions.ScriptJobs;

namespace Orbital7.Extensions.EntityFrameworkCore;

public abstract class DbContextMigrationScriptJobBase<TDbContext> :
    ScriptJobBase
    where TDbContext : DbContext
{
    public const string ARG_INITIALIZE = "-Initialize";

    protected string[] Args { get; private set; }

    protected bool Initialize { get; private set; }

    protected IServiceProvider ServiceProvider { get; private set; }

    protected IServiceProviderInitializer Initializer { get; private set; }

    protected string InitializingParam =>
        this.Initialize ? " and *INITIALIZE*" : null;

    protected DbContextMigrationScriptJobBase(
        IServiceProvider serviceProvider,
        IServiceProviderInitializer initializer,
        bool initialize)
    {
        this.ServiceProvider = serviceProvider;
        this.Initializer = initializer;
        this.Initialize = initialize;
    }

    protected DbContextMigrationScriptJobBase(
        IServiceProvider serviceProvider,
        IServiceProviderInitializer initializer,
        bool initialize,
        string[] args) :
        this(serviceProvider, initializer, initialize)
    {
        this.Args = args;
    }

    protected abstract bool CanInitialize();

    public async override Task ExecuteAsync()
    {
        if (this.Initialize)
        {
            await DbContextMigrationHelper<TDbContext>.DeleteMigrateAndInitializeAsync(
                this.ServiceProvider,
                this.Initializer,
                this.CanInitialize);
        }
        else
        {
            await DbContextMigrationHelper<TDbContext>.MigrateAsync(
                this.ServiceProvider);
        }
    }
}