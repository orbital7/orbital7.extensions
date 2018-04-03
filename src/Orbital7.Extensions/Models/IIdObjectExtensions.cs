using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Orbital7.Extensions.Models
{
    public static class IIdObjectExtensions
    {
        public static bool Contains<T>(this List<T> list, Guid? id)
            where T : IIdObject
        {
            if (id.HasValue)
                return (from x in list
                        where x.Id == id
                        select x.Id).Count() > 0;
            else
                return false;
        }

        public static T Get<T>(this List<T> list, Guid? id)
            where T : class, IIdObject
        {
            if (id.HasValue)
                return (from x in list
                        where x.Id == id
                        select x).FirstOrDefault();
            else
                return null;
        }

        public static List<T> Gather<T>(this List<T> list, List<Guid> ids)
            where T : class, IIdObject
        {
            if (ids.Count > 0)
                return (from x in list
                        where ids.Contains(x.Id)
                        select x).ToList();
            else
                return new List<T>();
        }

        public static List<Guid> GatherIds<T>(this List<T> list)
            where T : IIdObject
        {
            return (from x in list
                    select x.Id).ToList();
        }
    }
}
