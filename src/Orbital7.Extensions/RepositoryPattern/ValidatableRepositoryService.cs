using System;
using System.Collections.Generic;
using System.Text;

namespace Orbital7.Extensions.RepositoryPattern
{
    public class ValidatableRepositoryService
    {
        protected IServiceProvider ServiceProvider { get; private set; }

        public ValidatableRepositoryService(IServiceProvider serviceProvider)
        {
            this.ServiceProvider = serviceProvider;
        }
    }

    public class ValidatableRepositoryService<T> : ValidatableRepositoryService where T : class
    {
        protected IValidatableRepository<T> Repository { get; private set; }

        public ValidatableRepositoryService(IServiceProvider serviceProvider, IValidatableRepository<T> repository)
            : base(serviceProvider)
        {
            this.Repository = repository;
        }
    }
}
