using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace Microsoft.AspNetCore.Http
{
    public static class HttpExtensions
    {
        public static string GetRelativeUri(this HttpRequest request)
        {
            return string.Format("{0}{1}", request.Path, request.QueryString.ToUriComponent());
        }

        public static string GetAbsoluteUri(this HttpRequest request)
        {
            return string.Format("{0}://{1}{2}{3}", request.Scheme, request.Host, request.Path, 
                request.QueryString.ToUriComponent());
        }

        public static string GetBaseUrl(this HttpRequest request)
        {
            return string.Format("{0}://{1}/", request.Scheme, request.Host);
        }

        public static void SetFailureState(this HttpResponse response)
        {
            response.StatusCode = (int)HttpStatusCode.BadRequest;
        }
    }
}
