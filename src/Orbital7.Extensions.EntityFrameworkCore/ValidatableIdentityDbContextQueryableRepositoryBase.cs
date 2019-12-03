using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Orbital7.Extensions.Models;
using Orbital7.Extensions.RepositoryPattern;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orbital7.Extensions.EntityFrameworkCore
{
    public abstract class ValidatableIdentityDbContextQueryableRepositoryBase<TEntity, TUser, TRole, TKey> 
        : IQueryableRepository<TEntity>
            where TEntity : class, IIdObject
            where TUser : IdentityUser<TKey>
            where TRole : IdentityRole<TKey>
            where TKey : IEquatable<TKey>
    {
        protected ValidatableIdentityDbContextBase<TUser, TRole, TKey> DbContext { get; private set; }
        protected DbSet<TEntity> DbSet { get; private set; }

        public ValidatableIdentityDbContextQueryableRepositoryBase(
            ValidatableIdentityDbContextBase<TUser, TRole, TKey> dbContext)
        {
            this.DbContext = dbContext;
            this.DbSet = dbContext.Set<TEntity>();
        }

        public virtual IQueryable<TEntity> AsQueryable()
        {
            return this.DbSet;
        }

        public virtual async Task<TEntity> GetAsync(
            IQueryable<TEntity> query,
            bool asReadOnly,
            List<string> includeNavigationPropertyPaths)
        {
            if (query != null)
            {
                return await query
                    .SetIncludes(includeNavigationPropertyPaths)
                    .FirstOrDefaultAsync(asReadOnly);
            }
            else
            {
                return null;
            }
        }

        public virtual async Task<TEntity> GetAsync(
            Guid? id,
            bool asReadOnly,
            List<string> includeNavigationPropertyPaths)
        {
            if (id.HasValue && id != Guid.Empty)
            {
                return await GetAsync((from x in this.DbSet
                                       where x.Id == id.Value
                                       select x), asReadOnly, includeNavigationPropertyPaths);
            }
            else
            {
                return null;
            }
        }

        public async Task<TDynamic> GetAsync<TDynamic>(
            IQueryable<TDynamic> query)
            where TDynamic : class
        {
            return await query.FirstOrDefaultAsync();
        }

        public async Task<TDynamic> GetAsync<TDynamic>(
            IQueryable<TDynamic> query,
            List<string> includeNavigationPropertyPaths)
            where TDynamic : class
        {
            return await query.SetIncludes(includeNavigationPropertyPaths).FirstOrDefaultAsync();
        }

        public virtual async Task<List<TEntity>> GatherAsync(
            IQueryable<TEntity> query,
            bool asReadOnly,
            List<string> includeNavigationPropertyPaths)
        {
            if (query != null)
            {
                return await query
                    .SetIncludes(includeNavigationPropertyPaths)
                    .ToListAsync(asReadOnly);
            }
            else
            {
                return new List<TEntity>();
            }
        }

        public virtual async Task<List<TEntity>> GatherAsync(
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

                // TODO: Refactor this to use SQL parameterization; need to ensure that additional 'where' 
                // clauses can be passed in to this method.
                var sql = string.Format("SELECT * FROM {0} WHERE {1} IN ({2})", GetTableName(),
                    (whereAndClause + " " + queryIdFieldName).Trim(), values.ToString());
                return await GatherAsync(this.DbSet.FromSql(sql), asReadOnly, includeNavigationPropertyPaths);
            }
            else
            {
                return new List<TEntity>();
            }
        }

        protected virtual string GetTableName()
        {
            return typeof(TEntity).Name.Pluralize();
        }

        public async Task<List<TDynamic>> GatherAsync<TDynamic>(
            IQueryable<TDynamic> query) where TDynamic : class
        {
            return await query.ToListAsync();
        }

        public async Task<List<TDynamic>> GatherAsync<TDynamic>(
            IQueryable<TDynamic> query,
            List<string> includeNavigationPropertyPaths) where TDynamic : class
        {
            return await query.SetIncludes(includeNavigationPropertyPaths).ToListAsync();
        }

        public async Task<int> CountAsync<TDynamic>(IQueryable<TDynamic> query)
        {
            return await query.CountAsync();
        }

        public virtual async Task<TEntity> FindAsync(
            Guid id)
        {
            return await this.DbSet.FindAsync(id);
        }
    }
}
