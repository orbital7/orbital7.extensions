namespace Orbital7.Extensions.Apis.BetterStackApi;

public record Pagination
{
    [JsonPropertyName("first")]
    public string? First { get; init; }

    [JsonPropertyName("last")]
    public string? Last { get; init; }

    [JsonPropertyName("prev")]
    public string? Prev { get; init; }

    [JsonPropertyName("next")]
    public string? Next { get; init; }
}
