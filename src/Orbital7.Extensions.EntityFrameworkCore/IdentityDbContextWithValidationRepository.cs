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
    public class IdentityDbContextWithValidationRepository<TEntity, TUser, TRole, TKey> : IValidatableRepository<TEntity>
        where TEntity : class, IIdObject
        where TUser : IdentityUser<TKey>
        where TRole : IdentityRole<TKey>
        where TKey : IEquatable<TKey>
    {
        protected IdentityDbContextWithValidation<TUser, TRole, TKey> DbContext { get; private set; }
        protected DbSet<TEntity> DbSet { get; private set; }

        public IdentityDbContextWithValidationRepository(
            IdentityDbContextWithValidation<TUser, TRole, TKey> dbContext)
        {
            this.DbContext = dbContext;
            this.DbSet = dbContext.Set<TEntity>();
        }

        public async virtual Task<TEntity> AddAsync(
            TEntity entity, 
            RepositorySaveAction save = RepositorySaveAction.No)
        {
            this.DbSet.Add(entity);
            await HandleSaveAsync(save);
            return entity;
        }

        public async virtual Task<TEntity> UpdateAsync(
            TEntity entity, 
            RepositorySaveAction save = RepositorySaveAction.No)
        {
            this.DbSet.Update(entity);
            await HandleSaveAsync(save);
            return entity;
        }

        public async virtual Task<TEntity> DeleteAsync(
            TEntity entity, 
            RepositorySaveAction save = RepositorySaveAction.No)
        {
            this.DbSet.Remove(entity);
            await HandleSaveAsync(save);
            return entity;
        }

        public virtual IQueryable<TEntity> Query(
            Expression<Func<TEntity, bool>> query, 
            bool asReadOnly,
            List<string> includeNavigationPropertyPaths)
        {
            return Complete(this.DbSet.Where(query), asReadOnly, includeNavigationPropertyPaths);
        }

        private IQueryable<TEntity> Complete(
            IQueryable<TEntity> query,
            bool asReadOnly,
            List<string> includeNavigationPropertyPaths)
        {
            if (includeNavigationPropertyPaths != null)
                foreach (var navigationPropertyPath in includeNavigationPropertyPaths)
                    query = query.Include(navigationPropertyPath);

            if (asReadOnly)
                return query.AsNoTracking();
            else
                return query;
        }

        public virtual IQueryable<TEntity> AsQueryable(
            bool asReadOnly,
            List<string> includeNavigationPropertyPaths)
        {
            return Complete(this.DbSet, asReadOnly, includeNavigationPropertyPaths);
        }

        protected virtual string GetTableName()
        {
            return typeof(TEntity).Name.Pluralize();
        }

        public virtual IQueryable<TEntity> QueryForContainsIds(
            IList ids, 
            bool asReadOnly,
            List<string> includeNavigationPropertyPaths, 
            string whereAndClause = "", 
            string queryIdFieldName = "Id")
        {
            if (ids.Count > 0)
            {
                var values = new StringBuilder();
                values.AppendFormat("'{0}'", ids[0]);
                for (int i = 1; i < ids.Count; i++)
                    values.AppendFormat(", '{0}'", ids[i]);

                var sql = String.Format("SELECT * FROM {0} WHERE {1}", GetTableName(), whereAndClause).Trim();
                sql += String.Format(" {0} IN ({1})", queryIdFieldName, values);

                return Complete(this.DbSet.FromSql(sql), asReadOnly, includeNavigationPropertyPaths);
            }
            else
            {
                return Enumerable.Empty<TEntity>().AsAsyncQueryable();
            }
        }

        public virtual IQueryable<TEntity> QueryForId(
            Guid? id, 
            bool asReadOnly,
            List<string> includeNavigationPropertyPaths)
        {
            if (id.HasValue && id != Guid.Empty)
            {
                var result = (from x in this.DbSet
                              where x.Id == id.Value
                              select x);
                return Complete(result, asReadOnly, includeNavigationPropertyPaths);
            }
            else
            {
                return Enumerable.Empty<TEntity>().AsAsyncQueryable();
            }
        }

        public virtual async Task SaveAsync()
        {
            await this.DbContext.SaveChangesAsync();
        }

        public virtual async Task ValidateAndSaveAsync()
        {
            await this.DbContext.ValidateAndSaveChangesAsync();
        }

        public virtual async Task<TEntity> FindAsync(
            Guid id)
        {
            return await this.DbSet.FindAsync(id);
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
