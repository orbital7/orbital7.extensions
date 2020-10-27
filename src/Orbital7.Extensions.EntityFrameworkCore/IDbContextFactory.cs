using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Orbital7.Extensions.EntityFrameworkCore
{
    public interface IDbContextFactory<TDbContext>
        where TDbContext : DbContext
    {
        TDbContext Create();
    }
}
