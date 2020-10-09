using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace System.ComponentModel.DataAnnotations
{
    public class HexColorLengthAttribute : StringLengthAttribute
    {
        public HexColorLengthAttribute()
            : base(6)
        {

        }
    }
}
