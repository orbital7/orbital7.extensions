using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Orbital7.Extensions.RepositoryPattern
{
    public interface IValidatableActionUserRepository<TEntity, TKey>
    {
        TEntity Add(
            TEntity entity,
            TKey actionUserId);

        Task<TEntity> AddAsync(
            TEntity entity,
            TKey actionUserId,
            RepositorySaveAction save = RepositorySaveAction.No);

        Task<TEntity> UpdateAsync(
            TEntity entity,
            TKey actionUserId,
            RepositorySaveAction save = RepositorySaveAction.No);

        TEntity Update(
            TEntity entity,
            TKey actionUserId);

        List<TEntity> UpdateItems(
            List<TEntity> existingItems,
            List<TEntity> updatedItems,
            TKey actionUserId);

        Task<TEntity> DeleteAsync(
            TEntity entity,
            TKey actionUserId,
            RepositorySaveAction save = RepositorySaveAction.No);

        TEntity Delete(
            TEntity entity,
            TKey actionUserId);

        IQueryable<TEntity> AsQueryable();

        Task<TEntity> GetAsync(
            Guid? id,
            bool asReadOnly,
            List<string> includeNavigationPropertyPaths);

        Task<TEntity> GetAsync(
            IQueryable<TEntity> query,
            bool asReadOnly,
            List<string> includeNavigationPropertyPaths);

        Task<TDynamic> GetAsync<TDynamic>(
            IQueryable<TDynamic> query);

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
            IQueryable<TDynamic> query);

        Task<int> CountAsync<TDynamic>(
            IQueryable<TDynamic> query);

        Task SaveAsync();

        Task ValidateAndSaveAsync();

        Task<TEntity> FindAsync(
            Guid id);
    }
}