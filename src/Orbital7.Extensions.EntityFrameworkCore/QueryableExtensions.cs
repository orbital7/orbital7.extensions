using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.EntityFrameworkCore
{
    public static class QueryableExtensions
    {
        public static async Task<List<TEntity>> ToListAsync<TEntity>(this IQueryable<TEntity> source, 
            bool asNoTracking) where TEntity : class
        {
            return asNoTracking ? await source.AsNoTracking().ToListAsync() : await source.ToListAsync();
        }

        public static async Task<TEntity> FirstOrDefaultAsync<TEntity>(this IQueryable<TEntity> source,
            bool asNoTracking) where TEntity : class
        {
            return asNoTracking ? await source.AsNoTracking().FirstOrDefaultAsync() : await source.FirstOrDefaultAsync();
        }
    }
}
