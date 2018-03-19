using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Orbital7.Extensions.Attributes
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
