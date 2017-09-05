using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.AspNetCore.Http
{
    public static class WebExtensions
    {
        public static void SetFailureState(this HttpResponse response)
        {
            response.StatusCode = (int)HttpStatusCode.BadRequest;
        }

        public static string GetURL(this HttpRequest request)
        {
            return "https://" + request.PathBase + "/";
        }
    }
}
