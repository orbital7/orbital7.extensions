﻿namespace Orbital7.Extensions.Integrations.BetterStackApi;

public interface IBetterStackClient :
    IApiClient
{
    string BearerToken { set; }
}
