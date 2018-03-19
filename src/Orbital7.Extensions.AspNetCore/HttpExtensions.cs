using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.AspNetCore.Http
{
    public static class HttpExtensions
    {
        public static string GetRelativeUri(this HttpRequest request)
        {
            return String.Format("{0}{1}", request.Path, request.QueryString.ToUriComponent());
        }

        public static string GetAbsoluteUri(this HttpRequest request)
        {
            return String.Format("{0}://{1}{2}{3}", request.Scheme, request.Host, request.Path, 
                request.QueryString.ToUriComponent());
        }
    }
}
