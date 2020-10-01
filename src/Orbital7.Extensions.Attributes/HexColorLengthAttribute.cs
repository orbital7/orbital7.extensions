using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Orbital7.Extensions.Attributes
{
    public class HexColorLengthAttribute : StringLengthAttribute
    {
        public HexColorLengthAttribute()
            : base(6)
        {

        }
    }
}
