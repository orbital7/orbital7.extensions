using System;
using System.Collections.Generic;
using System.Text;

namespace Orbital7.Extensions.Reporting
{
    public class ReportInfoAttribute : Attribute
    {
        public string Key { get; set; }

        public string Name { get; set; }

        public ReportInfoAttribute()
            : base()
        {

        }

        public ReportInfoAttribute(string key, string name)
            : this()
        {
            this.Key = key;
            this.Name = name;
        }

        public override string ToString()
        {
            return this.Key;
        }
    }
}
