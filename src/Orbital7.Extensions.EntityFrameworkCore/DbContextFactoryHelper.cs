namespace Microsoft.EntityFrameworkCore;

public static class DbContextFactoryHelper
{
    public static void ConfigureSqlServerOptionsBuilder(
        DbContextOptionsBuilder optionsBuilder,
        string connectionString,
        string migrationsAssemblyName)
    {
        optionsBuilder.EnableSensitiveDataLogging(true);
        optionsBuilder.UseSqlServer(
            connectionString,
            sqlOptions => sqlOptions
                .EnableRetryOnFailure(
                    maxRetryCount: 4,
                    maxRetryDelay: TimeSpan.FromSeconds(15),
                    errorNumbersToAdd: null)
                .MigrationsAssembly(migrationsAssemblyName));
    }
}
