using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Orbital7.Extensions.EntityFrameworkCore
{
    public interface IDbContextInitializer<TDbContext>
        where TDbContext : DbContext
    {
        Task InitializeAsync(TDbContext context);
    }
}
