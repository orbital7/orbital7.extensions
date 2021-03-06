﻿using Microsoft.AspNetCore.Identity;
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
    public abstract class ValidatableDbContextQueryableRepositoryBase<TDbContext, TEntity> 
        : IQueryableRepository<TEntity>
            where TDbContext : DbContext, IValidatableDbContext
            where TEntity : class, IIdObject
    {
        protected TDbContext DbContext { get; private set; }
        protected DbSet<TEntity> DbSet { get; private set; }

        protected ValidatableDbContextQueryableRepositoryBase(
            TDbContext dbContext)
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
            bool asReadOnly = true,
            List<string> includeNavigationPropertyPaths = null)
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
            bool asReadOnly = true,
            List<string> includeNavigationPropertyPaths = null)
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

        public async Task<Guid> GetAsync(
            IQueryable<Guid> query)
        {
            return await query.FirstOrDefaultAsync();
        }

        public async Task<Guid?> GetAsync(
            IQueryable<Guid?> query)
        {
            return await query.FirstOrDefaultAsync();
        }

        public async Task<bool> GetAsync(
            IQueryable<bool> query)
        {
            return await query.FirstOrDefaultAsync();
        }

        public async Task<bool?> GetAsync(
            IQueryable<bool?> query)
        {
            return await query.FirstOrDefaultAsync();
        }

        public async Task<int> GetAsync(
            IQueryable<int> query)
        {
            return await query.FirstOrDefaultAsync();
        }

        public async Task<int?> GetAsync(
            IQueryable<int?> query)
        {
            return await query.FirstOrDefaultAsync();
        }

        public async Task<long> GetAsync(
            IQueryable<long> query)
        {
            return await query.FirstOrDefaultAsync();
        }

        public async Task<long?> GetAsync(
            IQueryable<long?> query)
        {
            return await query.FirstOrDefaultAsync();
        }

        public async Task<decimal> GetAsync(
            IQueryable<decimal> query)
        {
            return await query.FirstOrDefaultAsync();
        }

        public async Task<decimal?> GetAsync(
            IQueryable<decimal?> query)
        {
            return await query.FirstOrDefaultAsync();
        }

        public async Task<double> GetAsync(
            IQueryable<double> query)
        {
            return await query.FirstOrDefaultAsync();
        }

        public async Task<double?> GetAsync(
            IQueryable<double?> query)
        {
            return await query.FirstOrDefaultAsync();
        }

        public async Task<DateTime> GetAsync(
            IQueryable<DateTime> query)
        {
            return await query.FirstOrDefaultAsync();
        }

        public async Task<DateTime?> GetAsync(
            IQueryable<DateTime?> query)
        {
            return await query.FirstOrDefaultAsync();
        }

        public async Task<TDynamic> GetAsync<TDynamic>(
            IQueryable<TDynamic> query)
            where TDynamic : class
        {
            return await query.FirstOrDefaultAsync();
        }

        public async Task<TDynamic> GetAsync<TDynamic>(
            IQueryable<TDynamic> query,
            List<string> includeNavigationPropertyPaths = null)
            where TDynamic : class
        {
            return await query.SetIncludes(includeNavigationPropertyPaths).FirstOrDefaultAsync();
        }

        public virtual async Task<List<Guid>> GatherIdsAsync(
            IQueryable<TEntity> query)
        {
            if (query != null)
            {
                return await query
                    .Select(x => x.Id)
                    .ToListAsync();
            }
            else
            {
                return new List<Guid>();
            }
        }

        public virtual async Task<List<TEntity>> GatherAsync(
            IQueryable<TEntity> query,
            bool asReadOnly = true,
            List<string> includeNavigationPropertyPaths = null)
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

        public virtual async Task<List<Guid>> GatherIdsAsync(
            IList ids,
            string additionalWhereClause = "",
            string queryIdFieldName = "Id")
        {
            if (ids.Count > 0)
            {
                var sql = GetSelectFromIdsSql(ids, additionalWhereClause, queryIdFieldName);
                return await GatherIdsAsync(this.DbSet.FromSqlRaw(sql));
            }
            else
            {
                return new List<Guid>();
            }
        }

        public virtual async Task<List<TEntity>> GatherAsync(
            IList ids,
            bool asReadOnly = true,
            List<string> includeNavigationPropertyPaths = null,
            string additionalWhereClause = "",
            string queryIdFieldName = "Id")
        {
            if (ids.Count > 0)
            {
                var sql = GetSelectFromIdsSql(ids, additionalWhereClause, queryIdFieldName);
                return await GatherAsync(this.DbSet.FromSqlRaw(sql), asReadOnly, includeNavigationPropertyPaths);
            }
            else
            {
                return new List<TEntity>();
            }
        }

        private string GetSelectFromIdsSql(
            IList ids,
            string additionalWhereClause = "",
            string queryIdFieldName = "Id")
        {
            var values = new StringBuilder();
            values.AppendFormat("'{0}'", ids[0]);
            for (int i = 1; i < ids.Count; i++)
                values.AppendFormat(", '{0}'", ids[i]);

            // TODO: Refactor this to use SQL parameterization; need to ensure that additional 'where' 
            // clauses can be passed in to this method.
            var whereClause = queryIdFieldName;
            if (!string.IsNullOrEmpty(additionalWhereClause))
                whereClause = additionalWhereClause + " AND " + whereClause;
            var sql = $"SELECT * FROM {GetTableName()} WHERE {whereClause} IN ({values})";

            return sql;
        }

        protected virtual string GetTableName()
        {
            return typeof(TEntity).Name.Pluralize();
        }

        public async Task<List<TDynamic>> GatherAsync<TDynamic>(
            IQueryable<TDynamic> query)
            where TDynamic : class
        {
            return await query.ToListAsync();
        }

        public async Task<List<Guid>> GatherAsync(
            IQueryable<Guid> query)
        {
            return await query.ToListAsync();
        }

        public async Task<List<Guid?>> GatherAsync(
            IQueryable<Guid?> query)
        {
            return await query.ToListAsync();
        }

        public async Task<List<TDynamic>> GatherAsync<TDynamic>(
            IQueryable<TDynamic> query,
            List<string> includeNavigationPropertyPaths = null) 
            where TDynamic : class
        {
            return await query.SetIncludes(includeNavigationPropertyPaths).ToListAsync();
        }

        public async Task<int> CountAsync<TDynamic>(
            IQueryable<TDynamic> query)
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
