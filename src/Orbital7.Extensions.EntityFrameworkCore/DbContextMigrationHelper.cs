namespace Orbital7.Extensions.EntityFrameworkCore;

public static class DbContextMigrationHelper<TDbContext>
    where TDbContext : DbContext
{
    public static async Task MigrateAsync(
        IServiceProvider serviceProvider)
    {
        using (var scope = serviceProvider.CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<TDbContext>();
            if (context.Database.IsRelational())
            {
                Console.Write("Migrating database...");
                await context.Database.MigrateAsync();
                Console.WriteLine("Success");
            }
        }
    }

    public static async Task DeleteMigrateAndInitializeAsync(
        IServiceProvider serviceProvider,
        IServiceProviderInitializer initializer,
        Func<bool> validateInitialize)
    {
        // Validate.
        ValidateInitialize(initializer, validateInitialize);

        // Delete the database.
        await ExecuteDeleteAsync(serviceProvider);

        // Migrate the database.
        await MigrateAsync(serviceProvider);

        // Initialize the database.
        await ExecuteInitializeAsync(serviceProvider, initializer);
    }

    public static async Task MigrateAndInitializeAsync(
        IServiceProvider serviceProvider,
        IServiceProviderInitializer initializer,
        Func<bool> validateInitialize)
    {
        // Validate.
        ValidateInitialize(initializer, validateInitialize);

        // Migrate the database.
        await MigrateAsync(serviceProvider);

        // Initialize the database.
        await ExecuteInitializeAsync(serviceProvider, initializer);
    }

    private static void ValidateInitialize(
        IServiceProviderInitializer initializer,
        Func<bool> validateInitialize)
    {
        if (initializer == null)
        {
            throw new Exception("Provided initializer is null");
        }
        else if (validateInitialize == null)
        {
            throw new Exception("Provided initialize validation function is null");
        }
        else if (!validateInitialize())
        {
            throw new Exception("Initialize action was not validated per provided validation function");
        }
    }

    private static async Task ExecuteInitializeAsync(
        IServiceProvider serviceProvider,
        IServiceProviderInitializer initializer)
    {
        using (var scope = serviceProvider.CreateScope())
        {
            Console.Write("Initializing database...");
            await initializer.InitializeAsync(scope.ServiceProvider);
            Console.WriteLine("Success");
        }
    }

    private static async Task ExecuteDeleteAsync(
        IServiceProvider serviceProvider)
    {
        using (var scope = serviceProvider.CreateScope())
        {
            Console.Write("Deleting database...");
            var context = scope.ServiceProvider.GetRequiredService<TDbContext>();
            await context.Database.EnsureDeletedAsync();
            Console.WriteLine("Success");
        }
    }
}
