namespace Orbital7.Extensions.Integrations;

public interface IApiClient
{
    Task<TResponse> SendGetRequestAsync<TResponse>(
        string url);

    Task<TResponse> SendDeleteRequestAsync<TResponse>(
        string url);

    Task<TResponse> SendPostRequestAsync<TRequest, TResponse>(
        string url,
        TRequest? request);

    Task<TResponse> SendPostRequestAsync<TResponse>(
        string url);

    Task<TResponse> SendPatchRequestAsync<TRequest, TResponse>(
        string url,
        TRequest? request);

    Task<TResponse> SendPatchRequestAsync<TResponse>(
        string url);

    Task<TResponse> SendPutRequestAsync<TRequest, TResponse>(
        string url,
        TRequest? request);

    Task<TResponse> SendPutRequestAsync<TResponse>(
        string url);

    Task<TResponse> SendPostRequestUrlEncodedAsync<TResponse>(
        string url,
        List<KeyValuePair<string, string>> request);
}
