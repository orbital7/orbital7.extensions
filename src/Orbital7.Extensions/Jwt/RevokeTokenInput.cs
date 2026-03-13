namespace Orbital7.Extensions.Jwt;

public record RevokeTokenInput
{
    [Required]
    public string? RefreshToken { get; set; }

    [MemberNotNull(
        nameof(RefreshToken))]
    public void AssertIsComplete()
    {
        ArgumentNullException.ThrowIfNull(this.RefreshToken, nameof(RefreshToken));
    }
}
