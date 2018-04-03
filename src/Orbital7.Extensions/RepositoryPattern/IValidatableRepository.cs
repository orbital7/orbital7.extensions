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

    public interface IValidatableRepository<T>
    {
        Task<T> AddAsync(T entity, RepositorySaveAction save = RepositorySaveAction.No);
        Task<T> UpdateAsync(T entity, RepositorySaveAction save = RepositorySaveAction.No);
        Task<T> DeleteAsync(T entity, RepositorySaveAction save = RepositorySaveAction.No);
        IQueryable<T> Query(Expression<Func<T, bool>> query);
        IQueryable<T> AsQueryable();
        IQueryable<T> QueryForContainsIds(IList ids, string whereAndClause = "", string queryIdFieldName = "Id");
        IQueryable<T> QueryForId(Guid? id);
        Task SaveAsync();
        Task ValidateAndSaveAsync();
        Task<T> FindAsync(Guid id);
    }
}
