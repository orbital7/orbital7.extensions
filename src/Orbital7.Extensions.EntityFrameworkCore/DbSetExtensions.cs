using Orbital7.Extensions.Models;
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
        public static IQueryable<T> CreateContainsIdsQuery<T>(
            this DbSet<T> dbSet, 
            IList ids, 
            string additionalWhereClause = "",
            string queryIdColumnName = "Id", 
            string tableNameOverride = null) 
            where T : class
        {
            if (ids.Count > 0)
            {
                var values = new StringBuilder();
                values.AppendFormat("'{0}'", ids[0]);
                for (int i = 1; i < ids.Count; i++)
                    values.AppendFormat(", '{0}'", ids[i]);

                // TODO: Refactor this to use SQL parameterization; need to ensure that additional 'where' 
                // clauses can be passed in to this method.
                var whereClause = queryIdColumnName;
                if (!string.IsNullOrEmpty(additionalWhereClause))
                    whereClause = additionalWhereClause + " AND " + whereClause;
                var tableName = tableNameOverride ?? typeof(T).Name.Pluralize();
                var sql = $"SELECT * FROM {tableName} WHERE {whereClause} IN ({values})";
                return dbSet.FromSqlRaw(sql);
            }
            else
            {
                return Enumerable.Empty<T>().AsAsyncQueryable();
            }
        }

        public static async Task<List<T>> GatherAsync<T>(
            this DbSet<T> dbSet, 
            IList ids, 
            bool asNoTracking,
            string additionalWhereClause = "", 
            string queryIdColumnName = "Id", 
            string tableNameOverride = null) 
            where T : class
        {
            return await CreateContainsIdsQuery(dbSet, ids, additionalWhereClause, queryIdColumnName,
                tableNameOverride).ToListAsync(asNoTracking);
        }

        public static List<T> UpdateItems<T>(
            this DbSet<T> dbSet,
            List<T> existingItems,
            List<T> updatedItems)
            where T : class, IIdObject
        {
            var deletedItems = new List<T>();

            // Update the new items.
            foreach (var item in updatedItems)
            {
                var existingItem = existingItems.Get(item.Id);
                if (existingItem != null)
                {
                    dbSet.Update(item);
                    existingItems.Remove(existingItem);
                }
                else
                {
                    dbSet.Add(item);
                }
            }

            // Remove the no-longer relevant existing items.
            foreach (var item in existingItems)
            {
                deletedItems.Add(item);
                dbSet.Remove(item);
            }

            return deletedItems;
        }
    }
}
