using System;
using System.Collections.Generic;
using System.Text;

namespace System.ComponentModel.DataAnnotations
{
    public class ZipCodeAttribute : RegularExpressionAttribute
    {
        public bool FullLength { get; private set; }

        public ZipCodeAttribute(bool fullLength = true)
            : base(fullLength ? @"^\d{5}(-\d{4})?$" : @"^(\d{5})?$")
        {
            this.FullLength = fullLength;
            this.ErrorMessage = "Invalid Zip Code";
        }
    }
}
