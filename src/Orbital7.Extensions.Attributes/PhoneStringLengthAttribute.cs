using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Orbital7.Extensions.Attributes
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
