using System;
using System.Collections.Generic;
using System.Text;

namespace System.ComponentModel.DataAnnotations
{
    public class OptimalStringLengthAttribute : StringLengthAttribute
    {
        public const int LENGTH = 4000;

        public OptimalStringLengthAttribute()
            : base(LENGTH)
        {

        }
    }
}
