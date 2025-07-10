namespace Orbital7.Extensions.Apis.BetterStackApi;

public class BetterStackApiClient :
    ApiClient, IBetterStackApiClient
{
    public string? BearerToken { private get; set; }

    // Use 20s timeout.
    protected override string? HttpClientName => HttpClientFactoryHelper.HTTP_CLIENT_NAME_TIMEOUT_20S;

    public BetterStackApiClient(
        IHttpClientFactory httpClientFactory,
        string? bearerToken = null) :
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
        string? message = null;

        if (responseBody.StartsWith("{\"errors\":\""))
        {
            var response = JsonSerializationHelper.DeserializeFromJson<SingleErrorsResponse>(responseBody);
            message = response.Errors;
        }
        else if (responseBody.StartsWith("{\"errors\":{\""))
        {
            var response = JsonSerializationHelper.DeserializeFromJson<MultipleErrorsResponse>(responseBody);
            message = response.Errors?
                .Select(x => $"{x.Key}: {x.Value.ToList().ToString(", ")}")
                .ToList()
                .ToString("; ");
        }

        return new Exception(message ?? responseBody);
    }
}
