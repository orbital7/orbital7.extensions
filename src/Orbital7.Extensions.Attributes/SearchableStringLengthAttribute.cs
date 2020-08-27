using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace System.ComponentModel.DataAnnotations
{
    public class SearchableStringLengthAttribute : StringLengthAttribute
    {
        public SearchableStringLengthAttribute()
            : base(128)
        {

        }
    }
}
