using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace System.ComponentModel.DataAnnotations
{
    public class PasswordAttribute : DataTypeAttribute
    {
        public PasswordAttribute()
            : base(DataType.Password)
        {
            
        }
    }
}
