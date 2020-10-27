using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Microsoft.EntityFrameworkCore
{
    public static class ChangeTrackerExtensions
    {
        // Source: https://weblogs.asp.net/ricardoperes/implementing-missing-features-in-entity-framework-core-part-3
        public static void Validate(
            this ChangeTracker changeTracker,
            IServiceProvider serviceProvider)
        {
            var items = new Dictionary<object, object>();

            foreach (var entry in changeTracker.Entries().Where(e => (e.State == EntityState.Added) || (e.State == EntityState.Modified)))
            {
                var entity = entry.Entity;
                var context = new ValidationContext(entity, serviceProvider, items);
                var results = new List<ValidationResult>();

                if (Validator.TryValidateObject(entity, context, results, true) == false)
                {
                    foreach (var result in results)
                    {
                        if (result != ValidationResult.Success)
                        {
                            throw new ValidationException(result.ErrorMessage);
                        }
                    }
                }
            }
        }
    }
}
