namespace Orbital7.Extensions.Apis.BetterStack;

public abstract class SingleDataResponseBase<TData>
{
    [JsonPropertyName("data")]
    public TData Data { get; set; }
}
