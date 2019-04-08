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
        : IQueryableRepository<TEntity>
    {
        TEntity Add(
            TEntity entity);

        Task<TEntity> AddAsync(
            TEntity entity, 
            RepositorySaveAction save = RepositorySaveAction.No);

        TEntity Update(
            TEntity entity);

        Task<TEntity> UpdateAsync(
            TEntity entity, 
            RepositorySaveAction save = RepositorySaveAction.No);

        List<TEntity> UpdateItems(
            List<TEntity> existingItems,
            List<TEntity> updatedItems);

        TEntity Delete(
            TEntity entity);

        Task<TEntity> DeleteAsync(
            TEntity entity, 
            RepositorySaveAction save = RepositorySaveAction.No);

        Task SaveAsync();

        Task ValidateAndSaveAsync();

        
    }
}
