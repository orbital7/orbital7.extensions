using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orbital7.Extensions.EntityFrameworkCore
{
    public abstract class IdentityDbContextWithValidation<TUser, TRole, TKey> : IdentityDbContext<TUser, TRole, TKey>
        where TUser : IdentityUser<TKey>
        where TRole : IdentityRole<TKey>
        where TKey : IEquatable<TKey>
    {
        public IdentityDbContextWithValidation(DbContextOptions options)
            : base(options)
        {

        }

        // NB: This method relies on the convention that the table names are class names + "s" or "ies" for class names ending in "y".
        public async Task<List<T>> GatherAsync<T>(List<Guid> ids, bool asNoTracking, string idColumnName = "Id") where T : class
        {
            var className = typeof(T).Name;
            var tableName = className.EndsWith("y") ? className.PruneEnd(1) + "ies" : className + "s";
            
            var values = new StringBuilder();
            values.AppendFormat("'{0}'", ids[0]);
            for (int i = 1; i < ids.Count; i++)
                values.AppendFormat(", '{0}'", ids[i]);

            var sql = string.Format("SELECT * FROM {0} WHERE {1} IN ({2})", tableName, idColumnName, values);

            // TODO: See https://github.com/aspnet/EntityFramework/issues/1862
            if (asNoTracking)
                return await Set<T>().FromSql(sql).AsNoTracking().ToListAsync();
            else
                return await Set<T>().FromSql(sql).ToListAsync();
        }

        public int ValidateAndSaveChanges(bool acceptAllChangesOnSuccess = true)
        {
            Validate();
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        public async Task<int> ValidateAndSaveChangesAsync(bool acceptAllChangesOnSuccess = true)
        {
            Validate();
            return await base.SaveChangesAsync(acceptAllChangesOnSuccess);
        }

        // Source: https://weblogs.asp.net/ricardoperes/implementing-missing-features-in-entity-framework-core-part-3
        private void Validate()
        {
            var serviceProvider = this.GetService<IServiceProvider>();
            var items = new Dictionary<object, object>();

            foreach (var entry in this.ChangeTracker.Entries().Where(e => (e.State == EntityState.Added) || (e.State == EntityState.Modified)))
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
