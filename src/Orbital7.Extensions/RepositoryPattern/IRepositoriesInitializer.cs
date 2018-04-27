using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Orbital7.Extensions.RepositoryPattern
{
    public interface IRepositoriesInitializer
    {
        Task InitializeAsync(IServiceProvider serviceProvider);
    }
}
