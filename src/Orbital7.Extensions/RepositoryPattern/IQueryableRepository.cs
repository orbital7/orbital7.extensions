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
            bool asReadOnly,
            List<string> includeNavigationPropertyPaths);

        Task<TEntity> GetAsync(
            IQueryable<TEntity> query,
            bool asReadOnly,
            List<string> includeNavigationPropertyPaths);

        Task<Guid> GetAsync(
            IQueryable<Guid> query);

        Task<Guid?> GetAsync(
            IQueryable<Guid?> query);

        Task<TDynamic> GetAsync<TDynamic>(
            IQueryable<TDynamic> query) 
            where TDynamic : class;

        Task<TDynamic> GetAsync<TDynamic>(
            IQueryable<TDynamic> query,
            List<string> includeNavigationPropertyPaths) 
            where TDynamic : class;

        Task<List<TEntity>> GatherAsync(
            IList ids,
            bool asReadOnly,
            List<string> includeNavigationPropertyPaths,
            string whereAndClause = "",
            string queryIdFieldName = "Id");

        Task<List<TEntity>> GatherAsync(
            IQueryable<TEntity> query,
            bool asReadOnly,
            List<string> includeNavigationPropertyPaths);

        Task<List<TDynamic>> GatherAsync<TDynamic>(
            IQueryable<TDynamic> query) 
            where TDynamic : class;

        Task<List<TDynamic>> GatherAsync<TDynamic>(
            IQueryable<TDynamic> query,
            List<string> includeNavigationPropertyPaths) 
            where TDynamic : class;

        Task<int> CountAsync<TDynamic>(
            IQueryable<TDynamic> query);
    }
}
