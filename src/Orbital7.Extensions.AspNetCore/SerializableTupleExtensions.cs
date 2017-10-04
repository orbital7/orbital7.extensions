using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.AspNetCore.Mvc
{
    public static class SerializableTupleExtensions
    {
        public static SelectList ToSelectList<T1, T2>(this IEnumerable<SerializableTuple<T1, T2>> list)
        {
            return new SelectList(list, "Item1", "Item2");
        }

        public static SelectList ToSelectList<T1, T2>(this IEnumerable<SerializableTuple<T1, T2>> list, T1 selected)
        {
            return new SelectList(list, "Item1", "Item2", selected);
        }
    }
}
