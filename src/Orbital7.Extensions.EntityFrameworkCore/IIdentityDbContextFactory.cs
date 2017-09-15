using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Orbital7.Extensions.EntityFrameworkCore
{
    public interface IIdentityDbContextFactory<T, TUser, TRole, TKey>
        where T : IdentityDbContext<TUser, TRole, TKey>
        where TUser : IdentityUser<TKey>
        where TRole : IdentityRole<TKey>
        where TKey : IEquatable<TKey>
    {
        T Create();
    }
}
