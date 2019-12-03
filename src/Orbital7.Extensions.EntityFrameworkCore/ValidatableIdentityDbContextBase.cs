using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orbital7.Extensions.EntityFrameworkCore
{
    public abstract class ValidatableIdentityDbContextBase<TUser, TRole, TKey> 
        : IdentityDbContext<TUser, TRole, TKey>
            where TUser : IdentityUser<TKey>
            where TRole : IdentityRole<TKey>
            where TKey : IEquatable<TKey>
    {
        protected ValidatableIdentityDbContextBase(
            DbContextOptions options)
            : base(options)
        {

        }

        public int ValidateAndSaveChanges(
            bool acceptAllChangesOnSuccess = true)
        {
            Validate();
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        public async Task<int> ValidateAndSaveChangesAsync(
            bool acceptAllChangesOnSuccess = true)
        {
            Validate();
            return await base.SaveChangesAsync(acceptAllChangesOnSuccess);
        }

        // Source: https://weblogs.asp.net/ricardoperes/implementing-missing-features-in-entity-framework-core-part-3
        public void Validate()
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
