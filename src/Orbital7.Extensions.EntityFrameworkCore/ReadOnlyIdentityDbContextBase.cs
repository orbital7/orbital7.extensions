using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orbital7.Extensions.EntityFrameworkCore
{
    public abstract class ReadOnlyIdentityDbContextBase<TUser, TRole> :
        IdentityDbContext<TUser, TRole, Guid>
        where TUser : IdentityUser<Guid>
        where TRole : IdentityRole<Guid>
    {
        protected ReadOnlyIdentityDbContextBase()
        {

        }

        protected ReadOnlyIdentityDbContextBase(
            DbContextOptions options)
            : base(options)
        {

        }

        public override int SaveChanges()
        {
            return ThrowExceptionOnSave();
        }

        public override int SaveChanges(
            bool acceptAllChangesOnSuccess)
        {
            return ThrowExceptionOnSave();
        }

        public override async Task<int> SaveChangesAsync(
            bool acceptAllChangesOnSuccess,
            CancellationToken cancellationToken = default)
        {
            await Task.CompletedTask;
            return ThrowExceptionOnSave();
        }

        public override async Task<int> SaveChangesAsync(
            CancellationToken cancellationToken = default)
        {
            await Task.CompletedTask;
            return ThrowExceptionOnSave();
        }

        protected override void ConfigureConventions(
            ModelConfigurationBuilder builder)
        {
            base.ConfigureConventions(builder);
            builder.SetDefaults();
        }

        protected override void OnModelCreating(
            ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.SetDefaults();

            // TODO: Should this use NoTracking or NoTrackingWithIdentityResolution?
            this.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTrackingWithIdentityResolution;
        }

        private int ThrowExceptionOnSave()
        {
            throw new Exception("Saving is not permitted on a read-only context");
        }
    }
}
