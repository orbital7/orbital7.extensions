namespace Orbital7.Extensions.Jwt;

public class ObtainTokenInput
{
    [Required]
    public string? Username { get; set; }

    [Required]
    public string? Password { get; set; }

    [Required]
    public string? Description { get; set; }

    [MemberNotNull(
        nameof(Username),
        nameof(Password),
        nameof(Description))]
    public void AssertIsComplete()
    {
        ArgumentNullException.ThrowIfNull(this.Username, nameof(Username));
        ArgumentNullException.ThrowIfNull(this.Password, nameof(Password));
        ArgumentNullException.ThrowIfNull(this.Description, nameof(Description));
    }
}
