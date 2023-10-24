namespace Orbital7.Extensions.Integrations.BetterStackApi;

public abstract class SingleDataResponseBase<TData>
{
    [JsonPropertyName("data")]
    public TData Data { get; set; }
}
