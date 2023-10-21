namespace Orbital7.Extensions.Apis.BetterStack;

public class HeartbeatRequest
{
    [JsonPropertyName("name")]
    public string Name { get; set; }

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
    public string HeartbeatGroupId { get; set; }

    [JsonPropertyName("sort_index")]
    public int? SortIndex { get; set; }

    [JsonPropertyName("paused")]
    public bool? Paused { get; set; }

    [JsonPropertyName("policy_id")]
    public string PolicyId { get; set; }
}
