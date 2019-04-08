using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Orbital7.Extensions.Models;
using Orbital7.Extensions.RepositoryPattern;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Orbital7.Extensions.EntityFrameworkCore
{
    public abstract class ValidatableIdentityDbContextValidatableRepositoryBase<TEntity, TUser, TRole, TKey> 
        : ValidatableIdentityDbContextQueryableRepositoryBase<TEntity, TUser, TRole, TKey>, IValidatableRepository<TEntity>
            where TEntity : class, IIdObject
            where TUser : IdentityUser<TKey>
            where TRole : IdentityRole<TKey>
            where TKey : IEquatable<TKey>
    {
        public ValidatableIdentityDbContextValidatableRepositoryBase(
            ValidatableIdentityDbContextBase<TUser, TRole, TKey> dbContext)
            : base(dbContext)
        {
            
        }

        public virtual TEntity Add(
            TEntity entity)
        {
            this.DbSet.Add(entity);
            return entity;
        }

        public async virtual Task<TEntity> AddAsync(
            TEntity entity, 
            RepositorySaveAction save = RepositorySaveAction.No)
        {
            Add(entity);
            await HandleSaveAsync(save);
            return entity;
        }

        public virtual TEntity Update(
            TEntity entity)
        {
            this.DbSet.Update(entity);
            return entity;
        }

        public virtual List<TEntity> UpdateItems(
            List<TEntity> existingItems,
            List<TEntity> updatedItems)
        {
            return this.DbSet.UpdateItems(existingItems, updatedItems);
        }

        public async virtual Task<TEntity> UpdateAsync(
            TEntity entity, 
            RepositorySaveAction save = RepositorySaveAction.No)
        {
            Update(entity);
            await HandleSaveAsync(save);
            return entity;
        }

        public virtual TEntity Delete(
            TEntity entity)
        {
            this.DbSet.Remove(entity);
            return entity;
        }

        public async virtual Task<TEntity> DeleteAsync(
            TEntity entity, 
            RepositorySaveAction save = RepositorySaveAction.No)
        {
            Delete(entity);
            await HandleSaveAsync(save);
            return entity;
        }

        public virtual async Task SaveAsync()
        {
            await this.DbContext.SaveChangesAsync();
        }

        public virtual async Task ValidateAndSaveAsync()
        {
            await this.DbContext.ValidateAndSaveChangesAsync();
        }

        protected virtual async Task HandleSaveAsync(
            RepositorySaveAction save)
        {
            if (save == RepositorySaveAction.Yes)
                await SaveAsync();
            else if (save == RepositorySaveAction.YesWithValidation)
                await ValidateAndSaveAsync();
        }
    }
}
