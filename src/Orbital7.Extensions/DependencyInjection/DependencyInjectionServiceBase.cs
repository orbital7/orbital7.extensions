using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Extensions.DependencyInjection
{
    public abstract class DependencyInjectionServiceBase
    {
        protected IServiceProvider ServiceProvider { get; private set; }

        public DependencyInjectionServiceBase(IServiceProvider serviceProvider)
        {
            this.ServiceProvider = serviceProvider;
        }
    }
}
