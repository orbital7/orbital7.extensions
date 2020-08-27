using System;
using System.Collections.Generic;
using System.Text;

namespace System.ComponentModel.DataAnnotations
{
    public class ZipCodeStringLengthAttribute : StringLengthAttribute
    {
        public bool FullLength { get; private set; }

        public ZipCodeStringLengthAttribute(bool fullLength = true)
            : base(fullLength ? 10 : 5)
        {
            this.FullLength = fullLength;
        }
    }
}
