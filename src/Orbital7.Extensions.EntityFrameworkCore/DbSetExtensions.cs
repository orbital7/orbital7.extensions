using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.EntityFrameworkCore
{
    public static class DbSetExtensions
    {
        public static IQueryable<T> CreateContainsIdsQuery<T>(this DbSet<T> dbSet, IList ids, string whereAndClause = "",
            string queryIdColumnName = "Id", string tableNameOverride = null) where T : class
        {
            if (ids.Count > 0)
            {
                // NB: This assumes classname convention-based table naming.
                var tableName = tableNameOverride;
                if (String.IsNullOrEmpty(tableName))
                {
                    var className = typeof(T).Name;
                    if (className.EndsWith("y"))
                        tableName = className.PruneEnd(1) + "ies";
                    else if (className.EndsWith("is"))
                        tableName = className.PruneEnd(2) + "es";
                    else
                        tableName = className + "s";
                }

                var values = new StringBuilder();
                values.AppendFormat("'{0}'", ids[0]);
                for (int i = 1; i < ids.Count; i++)
                    values.AppendFormat(", '{0}'", ids[i]);

                var sql = String.Format("SELECT * FROM {0} WHERE {1}", tableName, whereAndClause).Trim();
                sql += String.Format(" {0} IN ({1})", queryIdColumnName, values);

                return dbSet.FromSql(sql);
            }
            else
            {
                return Enumerable.Empty<T>().AsAsyncQueryable();
            }
        }

        public static async Task<List<T>> GatherAsync<T>(this DbSet<T> dbSet, IList ids, bool asNoTracking, 
            string whereAndClause = "", string queryIdColumnName = "Id", string tableNameOverride = null) where T : class
        {
            return await CreateContainsIdsQuery(dbSet, ids, whereAndClause, queryIdColumnName, 
                tableNameOverride).ToListAsync(asNoTracking);
        }
    }
}
