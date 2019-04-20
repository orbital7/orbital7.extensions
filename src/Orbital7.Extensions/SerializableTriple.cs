using System;
using System.Collections.Generic;
using System.Text;

namespace System
{
    public class SerializableTriple<T1, T2, T3>
    {
        public T1 Item1 { get; set; }

        public T2 Item2 { get; set; }

        public T3 Item3 { get; set; }

        public string Tag { get; set; }

        public SerializableTriple()
        {

        }

        public SerializableTriple(T1 item1, T2 item2, T3 item3)
            : this()
        {
            this.Item1 = item1;
            this.Item2 = item2;
            this.Item3 = item3;
        }

        public SerializableTriple(T1 item1, T2 item2, T3 item3, string tag)
            : this(item1, item2, item3)
        {
            this.Tag = tag;
        }

        public override string ToString()
        {
            return string.Format("{0}, {1}, {2}",
                this.Item1 != null ? this.Item1.ToString() : "null",
                this.Item2 != null ? this.Item2.ToString() : "null",
                this.Item3 != null ? this.Item3.ToString() : "null");
        }
    }
}
