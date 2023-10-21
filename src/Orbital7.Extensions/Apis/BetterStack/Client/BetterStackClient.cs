namespace Orbital7.Extensions.Apis.BetterStack;

public class BetterStackClient :
    ApiClient, IBetterStackClient
{
    private string BearerToken { get; set; }

    public BetterStackClient(
        string bearerToken)
    {
        this.BearerToken = bearerToken;
    }

    protected override void AddRequestHeaders(
        HttpRequestMessage httpRequest)
    {
        httpRequest.AddBearerTokenAuthorizationHeader(this.BearerToken);
    }
}
