using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System
{
    public static class ExceptionExtensions
    {
        public static string FlattenMessages(this Exception ex, string delim = "; ")
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(ex.Message);

            var innerEx = ex.InnerException;
            while (innerEx != null)
            {
                sb.Append(delim);
                sb.Append(innerEx.Message);
                innerEx = innerEx.InnerException;
            }

            return sb.ToString();
        }
    }
}
