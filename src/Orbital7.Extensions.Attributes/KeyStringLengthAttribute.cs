using System;
using System.Collections.Generic;
using System.Text;

namespace System.ComponentModel.DataAnnotations
{
    public class KeyStringLengthAttribute : StringLengthAttribute
    {
        public KeyStringLengthAttribute()
            : base(50)
        {

        }
    }
}
