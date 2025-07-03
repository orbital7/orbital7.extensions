namespace Orbital7.Extensions.Jwt;

public class RefreshTokenInput
{
    [Required]
    public string? RefreshToken { get; set; }

    [Required]
    public string? AccessToken { get; set; }

    [MemberNotNull(
        nameof(RefreshToken),
        nameof(AccessToken))]
    public void AssertIsComplete()
    {
        ArgumentNullException.ThrowIfNull(this.RefreshToken, nameof(RefreshToken));
        ArgumentNullException.ThrowIfNull(this.AccessToken, nameof(AccessToken));
    }
}
