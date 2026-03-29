namespace Orbital7.Extensions.Apis.BetterStackApi.Uptime;

public abstract record SingleDataResponseBase<TData>
{
    [JsonPropertyName("data")]
    public TData? Data { get; init; }
}
