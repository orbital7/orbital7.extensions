namespace Orbital7.Extensions.Integrations.BetterStackApi;

public enum HeartbeatStatus
{
    [EnumMember(Value = "paused")]
    Paused,

    [EnumMember(Value = "pending")]
    Pending,

    [EnumMember(Value = "up")]
    Up,

    [EnumMember(Value = "down")]
    Down,
}
