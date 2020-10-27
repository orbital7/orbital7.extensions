using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Orbital7.Extensions.EntityFrameworkCore
{
    public interface IValidatableDbContext
    {
        int ValidateAndSaveChanges(
            bool acceptAllChangesOnSuccess = true);

        Task<int> ValidateAndSaveChangesAsync(
            bool acceptAllChangesOnSuccess = true);

        void Validate();
    }
}
