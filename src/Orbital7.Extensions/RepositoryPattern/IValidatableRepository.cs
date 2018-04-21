using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Orbital7.Extensions.RepositoryPattern
{
    public enum RepositorySaveAction
    {
        No,

        Yes,

        YesWithValidation,
    }

    public interface IValidatableRepository<TEntity>
    {
        Task<TEntity> AddAsync(
            TEntity entity, 
            RepositorySaveAction save = RepositorySaveAction.No);

        Task<TEntity> UpdateAsync(
            TEntity entity, 
            RepositorySaveAction save = RepositorySaveAction.No);

        Task<TEntity> DeleteAsync(
            TEntity entity, 
            RepositorySaveAction save = RepositorySaveAction.No);

        IQueryable<TEntity> Query(
            Expression<Func<TEntity, bool>> query, 
            bool asReadOnly,
            List<string> includeNavigationPropertyPaths);

        IQueryable<TEntity> AsQueryable(
            bool asReadOnly,
            List<string> includeNavigationPropertyPaths);

        IQueryable<TEntity> QueryForContainsIds(
            IList ids, 
            bool asReadOnly, 
            List<string> includeNavigationPropertyPaths, 
            string whereAndClause = "", 
            string queryIdFieldName = "Id");

        IQueryable<TEntity> QueryForId(
            Guid? id, 
            bool asReadOnly,
            List<string> includeNavigationPropertyPaths);

        Task SaveAsync();

        Task ValidateAndSaveAsync();

        Task<TEntity> FindAsync(
            Guid id);
    }
}
