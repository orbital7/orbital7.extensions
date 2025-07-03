﻿namespace Orbital7.Extensions.Apis.BetterStackApi;

public abstract class SingleDataResponseBase<TData>
{
    [JsonPropertyName("data")]
    public TData? Data { get; set; }
}
