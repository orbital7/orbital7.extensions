﻿using Microsoft.AspNetCore.Identity;
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
        : IdentityDbContext<TUser, TRole, TKey>, IValidatableDbContext
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

        public void Validate()
        {
            this.ChangeTracker.Validate(this.GetService<IServiceProvider>());
        }
    }
}
