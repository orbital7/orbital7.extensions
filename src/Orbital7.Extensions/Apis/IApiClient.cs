namespace Orbital7.Extensions.Apis;

public interface IApiClient
{
    Task<TResponse> SendGetRequestAsync<TResponse>(
        string url,
        CancellationToken cancellationToken = default);

    Task<TResponse> SendDeleteRequestAsync<TResponse>(
        string url,
        CancellationToken cancellationToken = default);

    Task<TResponse> SendPostRequestAsync<TRequest, TResponse>(
        string url,
        TRequest? request,
        CancellationToken cancellationToken = default);

    Task<TResponse> SendPostRequestAsync<TResponse>(
        string url,
        CancellationToken cancellationToken = default);

    Task<TResponse> SendPatchRequestAsync<TRequest, TResponse>(
        string url,
        TRequest? request,
        CancellationToken cancellationToken = default);

    Task<TResponse> SendPatchRequestAsync<TResponse>(
        string url,
        CancellationToken cancellationToken = default);

    Task<TResponse> SendPutRequestAsync<TRequest, TResponse>(
        string url,
        TRequest? request,
        CancellationToken cancellationToken = default);

    Task<TResponse> SendPutRequestAsync<TResponse>(
        string url,
        CancellationToken cancellationToken = default);

    Task<TResponse> SendPostRequestUrlEncodedAsync<TResponse>(
        string url,
        List<KeyValuePair<string, string>> request,
        CancellationToken cancellationToken = default);
}
