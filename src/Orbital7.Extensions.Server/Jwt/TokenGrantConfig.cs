using System.Diagnostics.CodeAnalysis;

namespace Orbital7.Extensions.Jwt;

public class TokenGrantConfig
{
    [Required]
    public string? ValidAudience { get; set; }

    [Required]
    public string? ValidIssuer { get; set; }

    [Required]
    public string? SigningKey { get; set; }

    public TimeSpan? ExpiresAfter { get; set; }

    public TimeSpan? AccessTokenExpiresAfter { get; set; }

    [MemberNotNull(
        nameof(ValidAudience),
        nameof(ValidIssuer),
        nameof(SigningKey))]
    public void AssertIsComplete()
    {
        ArgumentNullException.ThrowIfNull(this.ValidAudience, nameof(ValidAudience));
        ArgumentNullException.ThrowIfNull(this.ValidIssuer, nameof(ValidIssuer));
        ArgumentNullException.ThrowIfNull(this.SigningKey, nameof(SigningKey));
    }
}
