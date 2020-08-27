using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.ComponentModel.DataAnnotations
{
    public class AdditionalInformationAttribute : Attribute
    {
        public string Value { get; private set; }

        public AdditionalInformationAttribute(string value)
            : base()
        {
            this.Value = value;
        }

        public override string ToString()
        {
            return this.Value;
        }
    }
}
