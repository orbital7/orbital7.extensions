using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.EntityFrameworkCore
{
    public static class QueryableExtensions
    {
        public static IQueryable<TEntity> SetIncludes<TEntity>(
            this IQueryable<TEntity> query,
            List<string> includeNavigationPropertyPaths)
            where TEntity : class
        {
            if (includeNavigationPropertyPaths != null)
                foreach (var navigationPropertyPath in includeNavigationPropertyPaths)
                    query = query.Include(navigationPropertyPath);

            return query;
        }

        public static async Task<List<TEntity>> ToListAsync<TEntity>(
            this IQueryable<TEntity> source, 
            bool asNoTracking) 
            where TEntity : class
        {
            return asNoTracking ? await source.AsNoTracking().ToListAsync() : await source.ToListAsync();
        }

        public static async Task<TEntity> FirstOrDefaultAsync<TEntity>(
            this IQueryable<TEntity> source,
            bool asNoTracking) 
            where TEntity : class
        {
            return asNoTracking ? await source.AsNoTracking().FirstOrDefaultAsync() : await source.FirstOrDefaultAsync();
        }
    }
}
