using Microsoft.Extensions.DependencyInjection;
using Orbital7.Extensions.ScriptJobs;

namespace Microsoft.EntityFrameworkCore;

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
        // Validate initialize.
        if (this.Initialize)
        {
            if (this.Initializer == null)
            {
                throw new Exception("Provided initializer is null");
            }
            else if (!this.CanInitialize())
            {
                throw new Exception("Initialize request was not verified");
            }
        }

        // Delete on initialize.
        if (this.Initialize)
        {
            using (var scope = this.ServiceProvider.CreateScope())
            {
                Console.Write("Deleting Database...");
                var context = scope.ServiceProvider.GetRequiredService<TDbContext>();
                context.Database.EnsureDeleted();
                Console.WriteLine("Success");
            }
        }

        // Migrate and initialize if requested.
        using (var scope = this.ServiceProvider.CreateScope())
        {
            // Migrate.
            Console.Write("Migrating...");
            var context = this.ServiceProvider.GetRequiredService<TDbContext>();
            context.Database.Migrate();
            Console.WriteLine("Success");

            // Initialize.
            if (this.Initialize)
            {
                Console.Write("Initializing...");
                await this.Initializer.InitializeAsync(scope.ServiceProvider);
                Console.WriteLine("Success");
            }
        }
    }
}