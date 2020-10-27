using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orbital7.Extensions.RepositoryPattern
{
    public interface IQueryableRepository<TEntity>
    {
        IQueryable<TEntity> AsQueryable();

        Task<TEntity> FindAsync(
            Guid id);

        Task<TEntity> GetAsync(
            Guid? id,
            bool asReadOnly = true,
            List<string> includeNavigationPropertyPaths = null);

        Task<TEntity> GetAsync(
            IQueryable<TEntity> query,
            bool asReadOnly = true,
            List<string> includeNavigationPropertyPaths = null);

        Task<Guid> GetAsync(
            IQueryable<Guid> query);

        Task<Guid?> GetAsync(
            IQueryable<Guid?> query);

        Task<bool> GetAsync(
            IQueryable<bool> query);

        Task<bool?> GetAsync(
            IQueryable<bool?> query);

        Task<int> GetAsync(
            IQueryable<int> query);

        Task<int?> GetAsync(
            IQueryable<int?> query);

        Task<long> GetAsync(
            IQueryable<long> query);

        Task<long?> GetAsync(
            IQueryable<long?> query);

        Task<decimal> GetAsync(
            IQueryable<decimal> query);

        Task<decimal?> GetAsync(
            IQueryable<decimal?> query);

        Task<double> GetAsync(
            IQueryable<double> query);

        Task<double?> GetAsync(
            IQueryable<double?> query);

        Task<DateTime> GetAsync(
            IQueryable<DateTime> query);

        Task<DateTime?> GetAsync(
            IQueryable<DateTime?> query);

        Task<TDynamic> GetAsync<TDynamic>(
            IQueryable<TDynamic> query) 
            where TDynamic : class;

        Task<TDynamic> GetAsync<TDynamic>(
            IQueryable<TDynamic> query,
            List<string> includeNavigationPropertyPaths = null) 
            where TDynamic : class;

        Task<List<Guid>> GatherIdsAsync(
            IList ids,
            string additionalWhereClause = "",
            string queryIdFieldName = "Id");

        Task<List<TEntity>> GatherAsync(
            IList ids,
            bool asReadOnly = true,
            List<string> includeNavigationPropertyPaths = null,
            string additionalWhereClause = "",
            string queryIdFieldName = "Id");

        Task<List<TEntity>> GatherAsync(
            IQueryable<TEntity> query,
            bool asReadOnly = true,
            List<string> includeNavigationPropertyPaths = null);

        Task<List<TDynamic>> GatherAsync<TDynamic>(
            IQueryable<TDynamic> query) 
            where TDynamic : class;

        Task<List<Guid>> GatherAsync(
            IQueryable<Guid> query);

        Task<List<Guid?>> GatherAsync(
            IQueryable<Guid?> query);

        Task<List<TDynamic>> GatherAsync<TDynamic>(
            IQueryable<TDynamic> query,
            List<string> includeNavigationPropertyPaths = null) 
            where TDynamic : class;

        Task<int> CountAsync<TDynamic>(
            IQueryable<TDynamic> query);
    }
}
