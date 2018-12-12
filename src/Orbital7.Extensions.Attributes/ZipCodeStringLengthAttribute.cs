using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Orbital7.Extensions.Attributes
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
