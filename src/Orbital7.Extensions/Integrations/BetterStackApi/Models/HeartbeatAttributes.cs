namespace Orbital7.Extensions.Integrations.BetterStackApi;

public class HeartbeatAttributes
{
    [JsonPropertyName("url")]
    public string? Url { get; set; }

    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("period")]
    public int? Period { get; set; }

    [JsonPropertyName("grace")]
    public int? Grace { get; set; }

    [JsonPropertyName("call")]
    public bool? Call { get; set; }

    [JsonPropertyName("sms")]
    public bool? Sms { get; set; }

    [JsonPropertyName("email")]
    public bool? Email { get; set; }

    [JsonPropertyName("push")]
    public bool? Push { get; set; }

    [JsonPropertyName("team_wait")]
    public int? TeamWait { get; set; }

    [JsonPropertyName("heartbeat_group_id")]
    public string? HeartbeatGroupId { get; set; }

    [JsonPropertyName("sort_index")]
    public int? SortIndex { get; set; }

    [JsonPropertyName("paused_at")]
    public DateTime? PausedAt { get; set; }

    [JsonPropertyName("created_at")]
    public DateTime? CreatedAt { get; set; }

    [JsonPropertyName("updated_at")]
    public DateTime? UpdatedAt { get; set; }

    [JsonPropertyName("paused")]
    public bool? Paused { get; set; }

    [JsonPropertyName("status")]
    public HeartbeatStatus? Status { get; set; }

    public override string ToString()
    {
        return $"{this.Name}: {this.Status}";
    }
}
