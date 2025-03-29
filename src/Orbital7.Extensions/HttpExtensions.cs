using System.Net.Http.Headers;

namespace System.Net.Http;

public static class HttpExtensions
{
    public static AuthenticationHeaderValue AddAuthorizationHeader(
        this HttpRequestMessage httpRequest,
        string scheme,
        string parameter)
    {
        if (parameter.HasText())
        {
            httpRequest.Headers.Authorization = new AuthenticationHeaderValue(
                scheme,
                parameter);
            return httpRequest.Headers.Authorization;
        }

        return null;
    }

    public static AuthenticationHeaderValue AddBearerTokenAuthorizationHeader(
        this HttpRequestMessage httpRequest,
        string bearerToken)
    {
        return httpRequest.AddAuthorizationHeader("Bearer", bearerToken);
    }
}
