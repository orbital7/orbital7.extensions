using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Orbital7.Extensions.RepositoryPattern
{
    public class ValidatableRepositoryService<TRepository, TEntity> 
        : DependencyInjectionServiceBase 
            where TRepository : class, IValidatableRepository<TEntity>
            where TEntity : class
    {
        protected TRepository Repository { get; private set; }

        public ValidatableRepositoryService(IServiceProvider serviceProvider, TRepository repository)
            : base(serviceProvider)
        {
            this.Repository = repository;
        }
    }
}
