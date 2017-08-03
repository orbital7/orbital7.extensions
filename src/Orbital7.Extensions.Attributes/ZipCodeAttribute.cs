using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Orbital7.Extensions.Attributes
{
    public class ZipCodeAttribute : RegularExpressionAttribute
    {
        public ZipCodeAttribute()
            : base(@"^\d{5}(-\d{4})?$")
        {
            this.ErrorMessage = "Invalid Zip Code";
        }
    }
}
