using System;
using System.Collections.Generic;
using System.Text;

namespace Orbital7.Extensions
{
    public class InvalidAccessException : Exception
    {
        public InvalidAccessException()
            : base()
        {

        }

        public InvalidAccessException(string message)
            : base(message)
        {

        }
    }
}
