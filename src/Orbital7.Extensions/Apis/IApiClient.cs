namespace Orbital7.Extensions.Apis;

public interface IApiClient
{
    Task<TResponse> SendGetRequestAsync<TResponse>(
        string requestUrl,
        CancellationToken cancellationToken = default);

    Task<TResponse> SendDeleteRequestAsync<TResponse>(
        string requestUrl,
        CancellationToken cancellationToken = default);

    Task<TResponse> SendPostRequestAsync<TRequest, TResponse>(
        string requestUrl,
        TRequest? request,
        CancellationToken cancellationToken = default);

    Task<TResponse> SendPostRequestAsync<TResponse>(
        string requestUrl,
        CancellationToken cancellationToken = default);

    Task<TResponse> SendPatchRequestAsync<TRequest, TResponse>(
        string requestUrl,
        TRequest? request,
        CancellationToken cancellationToken = default);

    Task<TResponse> SendPatchRequestAsync<TResponse>(
        string requestUrl,
        CancellationToken cancellationToken = default);

    Task<TResponse> SendPutRequestAsync<TRequest, TResponse>(
        string requestUrl,
        TRequest? request,
        CancellationToken cancellationToken = default);

    Task<TResponse> SendPutRequestAsync<TResponse>(
        string requestUrl,
        CancellationToken cancellationToken = default);

    Task<TResponse> SendPostRequestUrlEncodedAsync<TResponse>(
        string requestUrl,
        IEnumerable<KeyValuePair<string, string>> request,
        CancellationToken cancellationToken = default);
}
