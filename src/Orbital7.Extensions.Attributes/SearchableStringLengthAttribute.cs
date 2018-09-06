using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Orbital7.Extensions.Attributes
{
    public class SearchableStringLengthAttribute : StringLengthAttribute
    {
        public SearchableStringLengthAttribute()
            : base(128)
        {

        }
    }
}
