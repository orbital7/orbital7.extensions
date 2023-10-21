using System.Net.Http.Headers;

namespace System.Net.Http;

public static class HttpExtensions
{
    public static AuthenticationHeaderValue AddBearerTokenAuthorizationHeader(
        this HttpRequestMessage httpRequest,
        string bearerToken)
    {
        if (bearerToken.HasText())
        {
            httpRequest.Headers.Authorization = new AuthenticationHeaderValue(
                "Bearer",
                bearerToken);
            return httpRequest.Headers.Authorization;
        }

        return null;
    }
}
