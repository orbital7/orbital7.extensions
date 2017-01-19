using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace System.Web
{
    public static class WebExtensions
    {
        public static void SetFailureState(this HttpResponseBase response)
        {
            response.TrySkipIisCustomErrors = true;
            response.StatusCode = (int)HttpStatusCode.BadRequest;
        }

        public static string GetURL(this HttpRequestBase request)
        {
            return "https://" + request.Url.Authority + "/";
        }
    }
}
