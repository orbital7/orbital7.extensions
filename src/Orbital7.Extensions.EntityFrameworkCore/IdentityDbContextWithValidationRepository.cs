using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Orbital7.Extensions.Models;
using Orbital7.Extensions.RepositoryPattern;
using System;
using System.Collections;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Orbital7.Extensions.EntityFrameworkCore
{
    public class IdentityDbContextWithValidationRepository<TObject, TUser, TRole, TKey> : IValidatableRepository<TObject>
        where TObject : class, IIdObject
        where TUser : IdentityUser<TKey>
        where TRole : IdentityRole<TKey>
        where TKey : IEquatable<TKey>
    {
        protected IdentityDbContextWithValidation<TUser, TRole, TKey> DbContext { get; private set; }
        protected DbSet<TObject> DbSet { get; private set; }

        public IdentityDbContextWithValidationRepository(IdentityDbContextWithValidation<TUser, TRole, TKey> dbContext)
        {
            this.DbContext = dbContext;
            this.DbSet = dbContext.Set<TObject>();
        }

        public async virtual Task<TObject> AddAsync(TObject entity, RepositorySaveAction save = RepositorySaveAction.No)
        {
            this.DbSet.Add(entity);
            await HandleSaveAsync(save);
            return entity;
        }

        public async virtual Task<TObject> UpdateAsync(TObject entity, RepositorySaveAction save = RepositorySaveAction.No)
        {
            this.DbSet.Update(entity);
            await HandleSaveAsync(save);
            return entity;
        }

        public async virtual Task<TObject> DeleteAsync(TObject entity, RepositorySaveAction save = RepositorySaveAction.No)
        {
            this.DbSet.Remove(entity);
            await HandleSaveAsync(save);
            return entity;
        }

        public virtual IQueryable<TObject> Query(Expression<Func<TObject, bool>> query)
        {
            return this.DbSet.Where(query);
        }

        public virtual IQueryable<TObject> AsQueryable()
        {
            return this.DbSet;
        }

        protected virtual string GetTableName()
        {
            return typeof(TObject).Name.Pluralize();
        }

        public virtual IQueryable<TObject> QueryForContainsIds(IList ids, string whereAndClause = "", string queryIdFieldName = "Id")
        {
            if (ids.Count > 0)
            {
                var values = new StringBuilder();
                values.AppendFormat("'{0}'", ids[0]);
                for (int i = 1; i < ids.Count; i++)
                    values.AppendFormat(", '{0}'", ids[i]);

                var sql = String.Format("SELECT * FROM {0} WHERE {1}", GetTableName(), whereAndClause).Trim();
                sql += String.Format(" {0} IN ({1})", queryIdFieldName, values);

                return this.DbSet.FromSql(sql);
            }
            else
            {
                return Enumerable.Empty<TObject>().AsAsyncQueryable();
            }
        }

        public virtual IQueryable<TObject> QueryForId(Guid? id)
        {
            if (id.HasValue && id != Guid.Empty)
            {
                return (from x in this.DbSet
                        where x.Id == id.Value
                        select x);
            }
            else
            {
                return Enumerable.Empty<TObject>().AsAsyncQueryable();
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

        public virtual async Task<TObject> FindAsync(Guid id)
        {
            return await this.DbSet.FindAsync(id);
        }

        protected virtual async Task HandleSaveAsync(RepositorySaveAction save)
        {
            if (save == RepositorySaveAction.Yes)
                await SaveAsync();
            else if (save == RepositorySaveAction.YesWithValidation)
                await ValidateAndSaveAsync();
        }
    }
}
