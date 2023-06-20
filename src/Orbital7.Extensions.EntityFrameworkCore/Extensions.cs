using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Microsoft.EntityFrameworkCore
{
    public static class Extensions
    {
        public static ModelConfigurationBuilder SetDefaults(
            this ModelConfigurationBuilder builder)
        {
            builder.Properties<DateOnly>()
                .HaveConversion<DateOnlyConverter>()
                .HaveColumnType("date");

            builder.Properties<TimeOnly>()
                .HaveConversion<TimeOnlyConverter>()
                .HaveColumnType("time");

            return builder;
        }

        public static ModelBuilder SetDefaults(
            this ModelBuilder modelBuilder)
        {
            // Set default decimal precision.
            var decimalProperties = modelBuilder.GetPropertiesForType(typeof(decimal), typeof(decimal?));
            foreach (var property in decimalProperties)
            {
                property.SetPrecision(18);
                property.SetScale(2);
            }

            // Set property comparer for DateOnly.
            var dateOnlyProperties = modelBuilder.GetPropertiesForType(typeof(DateOnly), typeof(DateOnly?));
            foreach (var property in dateOnlyProperties)
            {
                property.SetValueComparer(new DateOnlyComparer());
            }

            // Set property comparer for TimeOnly.
            var timeOnlyProperties = modelBuilder.GetPropertiesForType(typeof(TimeOnly), typeof(TimeOnly?));
            foreach (var property in timeOnlyProperties)
            {
                property.SetValueComparer(new TimeOnlyComparer());
            }

            return modelBuilder;
        }

        public static IEnumerable<IMutableProperty> GetPropertiesForType(
            this ModelBuilder modelBuilder,
            Type type,
            Type nullableType)
        {
            var properties = modelBuilder.Model.GetEntityTypes()
                .SelectMany(t => t.GetProperties())
                .Where(p => p.ClrType == type || p.ClrType == nullableType);

            return properties;
        }
    }
}
