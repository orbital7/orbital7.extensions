namespace Orbital7.Extensions.Apis.XApi;

public record XApiConfig
{
    public required string ClientId { get; init; }

    public required string RedirectUri { get; init; }

    public required string Scope { get; init; }

    public required string CodeVerifier { get; init; }
}
