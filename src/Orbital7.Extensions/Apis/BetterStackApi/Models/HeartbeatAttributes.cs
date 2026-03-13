namespace Orbital7.Extensions.Apis.BetterStackApi;

public record HeartbeatAttributes
{
    [JsonPropertyName("url")]
    public string? Url { get; init; }

    [JsonPropertyName("name")]
    public string? Name { get; init; }

    [JsonPropertyName("period")]
    public int? Period { get; init; }

    [JsonPropertyName("grace")]
    public int? Grace { get; init; }

    [JsonPropertyName("call")]
    public bool? Call { get; init; }

    [JsonPropertyName("sms")]
    public bool? Sms { get; init; }

    [JsonPropertyName("email")]
    public bool? Email { get; init; }

    [JsonPropertyName("push")]
    public bool? Push { get; init; }

    [JsonPropertyName("team_wait")]
    public int? TeamWait { get; init; }

    [JsonPropertyName("heartbeat_group_id")]
    public string? HeartbeatGroupId { get; init; }

    [JsonPropertyName("sort_index")]
    public int? SortIndex { get; init; }

    [JsonPropertyName("paused_at")]
    public DateTime? PausedAt { get; init; }

    [JsonPropertyName("created_at")]
    public DateTime? CreatedAt { get; init; }

    [JsonPropertyName("updated_at")]
    public DateTime? UpdatedAt { get; init; }

    [JsonPropertyName("paused")]
    public bool? Paused { get; init; }

    [JsonPropertyName("status")]
    public HeartbeatStatus? Status { get; init; }

    public override string ToString()
    {
        return $"{this.Name}: {this.Status}";
    }
}
