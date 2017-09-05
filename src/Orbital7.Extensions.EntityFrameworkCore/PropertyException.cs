using System;
using System.Collections.Generic;
using System.Text;

namespace Orbital7.Extensions.EntityFrameworkCore
{
    public class PropertyException : Exception
    {
        public string PropertyName { get; set; }

        public PropertyException(string propertyName, string message)
            : base(message)
        {
            this.PropertyName = propertyName;
        }
    }
}
