using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace System
{
    public class SelectableTuple<T1, T2> : SerializableTuple<T1, T2>
    {
        public bool CanSelect { get; set; } = true;

        public bool IsSelected { get; set; } = false;

        public SelectableTuple()
            : base()
        {

        }

        public SelectableTuple(T1 item1, T2 item2)
            : base(item1, item2)
        {

        }

        public override string ToString()
        {
            return string.Format("{0} {1}", this.Item2.ToString(), this.IsSelected ? "[Selected]" : null).Trim();
        }
    }
}
