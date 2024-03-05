namespace Orbital7.Extensions.Integrations.BetterStackApi;

public class BetterStackClient :
    ApiClient, IBetterStackClient
{
    private string BearerToken { get; set; }

    public BetterStackClient(
        IHttpClientFactory httpClientFactory,
        string bearerToken) :
        base(httpClientFactory)
    {
        this.BearerToken = bearerToken;
    }

    protected override void AddRequestHeaders(
        HttpRequestMessage httpRequest)
    {
        httpRequest.AddBearerTokenAuthorizationHeader(this.BearerToken);
    }

    protected override Exception CreateUnsuccessfulResponseException(
        HttpResponseMessage httpResponse, 
        string responseBody)
    {
        string message = responseBody;

        if (responseBody.StartsWith("{\"errors\":\""))
        {
            var response = JsonSerializationHelper.DeserializeFromJson<SingleErrorsResponse>(responseBody);
            message = response.Errors;
        }
        else if (responseBody.StartsWith("{\"errors\":{\""))
        {
            var response = JsonSerializationHelper.DeserializeFromJson<MultipleErrorsResponse>(responseBody);
            message = response.Errors
                .Select(x => $"{x.Key}: {x.Value.ToList().ToString(", ")}")
                .ToList()
                .ToString("; ");
        }

        return new Exception(message);
    }
}
