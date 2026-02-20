namespace Orbital7.Extensions.Apis.BetterStackApi;

public enum HeartbeatStatus
{
    [JsonStringEnumMemberName("paused")]
    Paused,

    [JsonStringEnumMemberName("pending")]
    Pending,

    [JsonStringEnumMemberName("up")]
    Up,

    [JsonStringEnumMemberName("down")]
    Down,
}
