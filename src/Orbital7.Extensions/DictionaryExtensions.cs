using System;
using System.Collections.Generic;
using System.Text;

namespace System.Collections.Generic
{
    public static class DictionaryExtensions
    {
        public static Dictionary<T1, T2> FillWith<T1, T2>(this Dictionary<T1, T2> dictionary, T1 key, T2 value)
        {
            dictionary.Add(key, value);

            return dictionary;
        }

        public static Dictionary<T1, T2> FillWith<T1, T2>(this Dictionary<T1, T2> dictionary, List<SerializableTuple<T1, T2>> items)
        {
            foreach (var item in items)
                dictionary.Add(item.Item1, item.Item2);

            return dictionary;
        }
    }
}
