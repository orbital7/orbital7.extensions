namespace Orbital7.Extensions.Jwt;

public record RevokedTokenInfo
{
    public string? UserId { get; init; }

    public string? Description { get; init; }
}
