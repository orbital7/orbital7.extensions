using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Orbital7.Extensions.ScriptJobs;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Orbital7.Extensions.EntityFrameworkCore
{
    public abstract class IdentityDbContextMigrationScriptJob<T, TUser, TRole, TKey> : ScriptJob
        where T : IdentityDbContext<TUser, TRole, TKey>
        where TUser : IdentityUser<TKey>
        where TRole : IdentityRole<TKey>
        where TKey : IEquatable<TKey>
    {
        public const string ARG_INITIALIZE = "-Initialize";

        private string[] Args { get; set; }

        public IdentityDbContextMigrationScriptJob()
        {

        }

        public IdentityDbContextMigrationScriptJob(string[] args)
            : this()
        {
            this.Args = args;
        }

        protected bool Initialize { get; set; } = false;

        protected abstract T Create();

        protected abstract IIdentityDbContextInitializer<T, TUser, TRole, TKey> CreateInitializer();

        protected abstract void DeleteOnInitialize();

        protected virtual void DeleteDatabase()
        {
            Console.Write("Deleting Database...");
            using (var context = Create())
            {
                context.Database.EnsureDeleted();
                Console.WriteLine("Success");
            }
        }

        public async override Task ExecuteAsync()
        {
            IIdentityDbContextInitializer<T, TUser, TRole, TKey> initializer = null;
            if (this.Initialize)
               initializer = CreateInitializer();

            // Delete on initialize.
            if (this.Initialize)
                DeleteOnInitialize();

            // Migrate and initialize.
            Console.Write("Migrating...");
            using (var context = Create())
            {
                // Migrate.
                context.Database.Migrate();
                Console.WriteLine("Success");

                // Initialize.
                if (this.Initialize)
                {
                    Console.Write("Initializing...");
                    await initializer.InitializeAsync(context);
                    Console.WriteLine("Success");
                }
            }
        }
    }
}
