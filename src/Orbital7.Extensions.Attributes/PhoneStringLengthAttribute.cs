using System;
using System.Collections.Generic;
using System.Text;

namespace System.ComponentModel.DataAnnotations
{
    public class PhoneStringLengthAttribute : StringLengthAttribute
    {
        public bool AllowExtension { get; private set; }

        public PhoneStringLengthAttribute(bool allowExtension = true)
            : base(allowExtension ? 20 : 10)
        {
            this.AllowExtension = allowExtension;
        }
    }
}
