using Microsoft.AspNetCore.Http;

namespace Orbital7.Extensions.AspNetCore;

public static class HttpExtensions
{
    public static string GetRelativeUri(
        this HttpRequest request)
    {
        return string.Format("{0}{1}", request.Path, request.QueryString.ToUriComponent());
    }

    public static string GetAbsoluteUri(
        this HttpRequest request)
    {
        return string.Format("{0}://{1}{2}{3}", request.Scheme, request.Host, request.Path, 
            request.QueryString.ToUriComponent());
    }

    public static string GetBaseUrl(
        this HttpRequest request)
    {
        return string.Format("{0}://{1}/", request.Scheme, request.Host);
    }

    public static string? GetIPAddress(
        this HttpRequest request)
    {
        var ipAddress = request.HttpContext.Connection.RemoteIpAddress?.ToString();

        if (ipAddress == "::1")
        {
            ipAddress = "127.0.0.1";
        }

        return ipAddress;
    }

    public static void SetFailureState(
        this HttpResponse response)
    {
        response.StatusCode = (int)HttpStatusCode.BadRequest;
    }
}
