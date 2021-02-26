using Orbital7.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace System
{
    public static class SerializableTripleExtensions
    {
        public static T2 GetItem2<T1, T2, T3>(
            this List<SerializableTriple<T1, T2, T3>> list, 
            T1 item1)
        {
            return (from x in list
                    where (x.Item1 == null && item1 == null) || (x.Item1 != null && x.Item1.Equals(item1))
                    select x.Item2).SingleOrDefault();
        }

        public static List<T2> GatherItem2s<T1, T2, T3>(
            this List<SerializableTriple<T1, T2, T3>> list, 
            T1 item1)
        {
            return (from x in list
                    where (x.Item1 == null && item1 == null) || (x.Item1 != null && x.Item1.Equals(item1))
                    select x.Item2).ToList();
        }

        public static T1 GetItem1<T1, T2, T3>(
            this List<SerializableTriple<T1, T2, T3>> list, 
            T2 item2)
        {
            return (from x in list
                    where (x.Item2 == null && item2 == null) || (x.Item2 != null && x.Item2.Equals(item2))
                    select x.Item1).SingleOrDefault();
        }

        public static List<T1> GatherItem1s<T1, T2, T3>(
            this List<SerializableTriple<T1, T2, T3>> list, 
            T2 item2)
        {
            return (from x in list
                    where (x.Item2 == null && item2 == null) || (x.Item2 != null && x.Item2.Equals(item2))
                    select x.Item1).ToList();
        }
    }
}
