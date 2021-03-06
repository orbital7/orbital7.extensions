﻿using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Orbital7.Extensions.RepositoryPattern;
using Orbital7.Extensions.ScriptJobs;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Orbital7.Extensions.EntityFrameworkCore
{
    public abstract class DbContextMigrationScriptJobBase<TDbContext> : ScriptJobBase
        where TDbContext : DbContext
    {
        public const string ARG_INITIALIZE = "-Initialize";

        private string[] Args { get; set; }
        
        protected IServiceProvider ServiceProvider { get; private set; }

        protected DbContextMigrationScriptJobBase(
            IServiceProvider serviceProvider)
        {
            this.ServiceProvider = serviceProvider;
        }

        protected DbContextMigrationScriptJobBase(
            IServiceProvider serviceProvider, string[] args)
            : this(serviceProvider)
        {
            this.Args = args;
        }

        protected bool Initialize { get; set; } = false;

        protected abstract IRepositoriesInitializer CreateInitializer();

        protected abstract bool ValidateDeleteOnInitialize();

        protected virtual void DeleteDatabase()
        {
            Console.Write("Deleting Database...");
            using (var scope = this.ServiceProvider.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<TDbContext>();
                context.Database.EnsureDeleted();
                Console.WriteLine("Success");
            }
        }

        public async override Task ExecuteAsync()
        {
            IRepositoriesInitializer initializer = null;
            if (this.Initialize)
               initializer = CreateInitializer();

            // Delete on initialize.
            if (this.Initialize)
            {
                if (ValidateDeleteOnInitialize())
                    DeleteDatabase();
                else
                    throw new Exception("Delete-on-Initialize command was not validated");
            }

            // Migrate and initialize.
            Console.Write("Migrating...");
            using (var scope = this.ServiceProvider.CreateScope())
            {
                // Migrate.
                var context = this.ServiceProvider.GetRequiredService<TDbContext>();
                context.Database.Migrate();
                Console.WriteLine("Success");

                // Initialize.
                if (this.Initialize && initializer != null)
                {
                    Console.Write("Initializing...");
                    await initializer.InitializeAsync(scope.ServiceProvider);
                    Console.WriteLine("Success");
                }
            }
        }
    }
}
