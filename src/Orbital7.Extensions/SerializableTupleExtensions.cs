using Orbital7.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace System
{
    public static class SerializableTupleExtensions
    {
        public static List<SerializableTuple<T, string>> InsertEmptyTextItem<T>(
            this List<SerializableTuple<T, string>> list, 
            string emptyText = null)
        {
            if (!string.IsNullOrEmpty(emptyText))
                list.Insert(0, new SerializableTuple<T, string>(default, emptyText));

            return list;
        }

        public static List<SerializableTuple<string, string>> ToTupleList(
            this List<Enum> list, 
            bool sort, 
            string emptyText = null)
        {
            var tupleList = new List<SerializableTuple<string, string>>();
            foreach (var item in list)
                tupleList.Add(new SerializableTuple<string, string>(item.ToString(), item.ToDisplayString()));

            if (sort)
                tupleList = tupleList.OrderBy(x => x.Item2).ToList();

            return tupleList.InsertEmptyTextItem(emptyText);
        }

        public static T2 GetItem2<T1, T2>(
            this List<SerializableTuple<T1, T2>> list, 
            T1 item1)
        {
            return (from x in list
                    where (x.Item1 == null && item1 == null) || (x.Item1 != null && x.Item1.Equals(item1))
                    select x.Item2).SingleOrDefault();
        }

        public static List<T2> GatherItem2s<T1, T2>(
            this List<SerializableTuple<T1, T2>> list, 
            T1 item1)
        {
            return (from x in list
                    where (x.Item1 == null && item1 == null) || (x.Item1 != null && x.Item1.Equals(item1))
                    select x.Item2).ToList();
        }

        public static T1 GetItem1<T1, T2>(
            this List<SerializableTuple<T1, T2>> list, 
            T2 item2)
        {
            return (from x in list
                    where (x.Item2 == null && item2 == null) || (x.Item2 != null && x.Item2.Equals(item2))
                    select x.Item1).SingleOrDefault();
        }

        public static List<T1> GatherItem1s<T1, T2>(
            this List<SerializableTuple<T1, T2>> list, 
            T2 item2)
        {
            return (from x in list
                    where (x.Item2 == null && item2 == null) || (x.Item2 != null && x.Item2.Equals(item2))
                    select x.Item1).ToList();
        }
    }
}
