using Orbital7.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace System
{
    public static class SerializableTupleExtensions
    {
        public static List<SerializableTuple<Guid, string>> InsertEmptyTextItem(this List<SerializableTuple<Guid, string>> list, string emptyText = null)
        {
            if (!String.IsNullOrEmpty(emptyText))
                list.Insert(0, new SerializableTuple<Guid, string>(Guid.Empty, emptyText));

            return list;
        }

        public static List<SerializableTuple<string, string>> InsertEmptyTextItem(this List<SerializableTuple<string, string>> list, string emptyText = null)
        {
            if (!String.IsNullOrEmpty(emptyText))
                list.Insert(0, new SerializableTuple<string, string>(String.Empty, emptyText));

            return list;
        }

        public static List<SerializableTuple<string, string>> ToTupleList(this List<Enum> list, bool sort, string emptyText = null)
        {
            var tupleList = new List<SerializableTuple<string, string>>();
            foreach (var item in list)
                tupleList.Add(new SerializableTuple<string, string>(item.ToString(), item.ToDisplayString()));

            if (sort)
                tupleList = tupleList.OrderBy(x => x.Item2).ToList();

            return tupleList.InsertEmptyTextItem(emptyText);
        }
    }
}
