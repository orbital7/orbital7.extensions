using Polly;
using Polly.Retry;
using System.Net;
using System.Net.Http.Headers;

namespace Orbital7.Extensions.Apis;

public class ApiClient :
    IApiClient
{
    protected IHttpClientFactory HttpClientFactory { get; private set; }

    protected string? HttpClientName { get; private set; }

    protected virtual bool SerializeEnumsToStrings => true;

    protected virtual bool DeserializeEnumsFromStrings => true;

    protected virtual string CharSet => "utf-8";

    protected virtual int RetryCount => 2;

    protected virtual int SleepDurationBaseInSeconds => 2;

    // Policy to retry 2x (for a total of 3 attempts) on transient errors.
    protected virtual AsyncRetryPolicy<HttpResponseMessage> RetryPolicy => Policy
        .Handle<HttpRequestException>()
        .OrResult<HttpResponseMessage>(r =>
            r.StatusCode == HttpStatusCode.RequestTimeout ||
            r.StatusCode == HttpStatusCode.InternalServerError ||
            r.StatusCode == HttpStatusCode.BadGateway ||
            r.StatusCode == HttpStatusCode.ServiceUnavailable ||
            r.StatusCode == HttpStatusCode.GatewayTimeout)
        .WaitAndRetryAsync(
            retryCount: this.RetryCount,
            sleepDurationProvider: 
                attempt => TimeSpan.FromSeconds(Math.Pow(this.SleepDurationBaseInSeconds, attempt)));

    public ApiClient(
        IHttpClientFactory httpClientFactory,
        string? httpClientName = null)
    {
        this.HttpClientFactory = httpClientFactory;
        this.HttpClientName = httpClientName;
    }

    public async Task<TResponse> SendGetRequestAsync<TResponse>(
        string requestUrl,
        CancellationToken cancellationToken = default)
    {
        return await SendRequestAsync<object, TResponse>(
            HttpMethod.Get,
            requestUrl,
            null,
            cancellationToken);
    }

    public async Task<TResponse> SendDeleteRequestAsync<TResponse>(
        string requestUrl,
        CancellationToken cancellationToken = default)
    {
        return await SendRequestAsync<object, TResponse>(
            HttpMethod.Delete,
            requestUrl,
            null,
            cancellationToken);
    }

    public async Task<TResponse> SendPostRequestAsync<TRequest, TResponse>(
        string requestUrl,
        TRequest? request,
        CancellationToken cancellationToken = default)
    {
        return await SendRequestAsync<TRequest, TResponse>(
            HttpMethod.Post,
            requestUrl,
            request,
            cancellationToken);
    }

    public async Task<TResponse> SendPostRequestAsync<TResponse>(
        string requestUrl,
        CancellationToken cancellationToken = default)
    {
        return await SendRequestAsync<object, TResponse>(
            HttpMethod.Post,
            requestUrl,
            null,
            cancellationToken);
    }

    public async Task<TResponse> SendPatchRequestAsync<TRequest, TResponse>(
        string requestUrl,
        TRequest? request,
        CancellationToken cancellationToken = default)
    {
        return await SendRequestAsync<TRequest, TResponse>(
            HttpMethod.Patch,
            requestUrl,
            request,
            cancellationToken);
    }

    public async Task<TResponse> SendPatchRequestAsync<TResponse>(
        string requestUrl,
        CancellationToken cancellationToken = default)
    {
        return await SendRequestAsync<object, TResponse>(
            HttpMethod.Patch,
            requestUrl,
            null,
            cancellationToken);
    }

    public async Task<TResponse> SendPutRequestAsync<TRequest, TResponse>(
        string requestUrl,
        TRequest? request,
        CancellationToken cancellationToken = default)
    {
        return await SendRequestAsync<TRequest, TResponse>(
            HttpMethod.Put,
            requestUrl,
            request,
            cancellationToken);
    }

    public async Task<TResponse> SendPutRequestAsync<TResponse>(
        string requestUrl,
        CancellationToken cancellationToken = default)
    {
        return await SendRequestAsync<object, TResponse>(
            HttpMethod.Put,
            requestUrl,
            null,
            cancellationToken);
    }

    public async Task<TResponse> SendPostRequestUrlEncodedAsync<TResponse>(
        string requestUrl,
        IEnumerable<KeyValuePair<string, string>> request,
        CancellationToken cancellationToken = default)
    {
        return await ExecuteSendRequestAsync<TResponse>(
            HttpMethod.Post,
            requestUrl,
            new FormUrlEncodedContent(request),
            cancellationToken);
    }

    private async Task<TResponse> SendRequestAsync<TRequest, TResponse>(
        HttpMethod method,
        string requestUrl,
        TRequest? request,
        CancellationToken cancellationToken)
    {
        // Serialize the request body.
        string? requestBody = request != null ?
            ExecuteSerializeRequestBody(request) :
            null;

        // Create request body content.
        HttpContent? content = requestBody.HasText() ?
            new StringContent(requestBody)
            {
                Headers =
                {
                    ContentType = new MediaTypeHeaderValue("application/json", this.CharSet)
                }
            } : null;

        return await ExecuteSendRequestAsync<TResponse>(
            method, 
            requestUrl, 
            content, 
            cancellationToken);
    }

    private async Task<TResponse> ExecuteSendRequestAsync<TResponse>(
        HttpMethod method,
        string requestUrl,
        HttpContent? content,
        CancellationToken cancellationToken)
    {
        HttpResponseMessage httpResponse;

        // Use retry policy to send the request.
        if (this.RetryPolicy != null)
        {
            httpResponse = await this.RetryPolicy.ExecuteAsync(
                async (x) => await CreateAndSendRequestAsync(
                    method,
                    requestUrl, 
                    content, 
                    cancellationToken),
                cancellationToken);
        }
        // Else send the request without retry policy.
        else
        {
            httpResponse = await CreateAndSendRequestAsync(
                method,
                requestUrl,
                content,
                cancellationToken);
        }

        // Read the response.
        using (httpResponse)
        { 
            var responseBody = await httpResponse.Content.ReadAsStringAsync(cancellationToken);

            if (!httpResponse.IsSuccessStatusCode)
            {
                throw CreateUnsuccessfulResponseException(httpResponse, responseBody);
            }
            else
            {
                return ExecuteDeserializeResponseBody<TResponse>(httpResponse, responseBody);
            }
        }
    }

    private async Task<HttpResponseMessage> CreateAndSendRequestAsync(
         HttpMethod method,
         string requestUrl,
         HttpContent? content,
         CancellationToken cancellationToken)
    {
        var requestUri = new Uri(requestUrl);

        // Perform any pre-request creation logic.
        await BeforeCreateRequestAsync(
            requestUri,
            cancellationToken);

        // Create the request.
        var httpRequest = new HttpRequestMessage()
        {
            Method = method,
            RequestUri = requestUri,
            Content = content,
        };

        // Add the request headers.
        AddRequestHeaders(httpRequest);

        // Send it.
        using (var httpClient = this.HttpClientFactory.CreateClient(this.HttpClientName ?? string.Empty))
        {
            return await httpClient.SendAsync(httpRequest, cancellationToken);
        }
    }

    protected virtual Task BeforeCreateRequestAsync(
        Uri requestUri,
        CancellationToken cancellationToken)
    {
        // Nothing to do here in the base implementation.
        return Task.CompletedTask;
    }

    protected virtual void AddRequestHeaders(
        HttpRequestMessage httpRequest)
    {
        // Nothing to do here in the base implementation.
    }

    // Use ProblemDetailsResponse / ProblemDetailsExecption by default.
    protected virtual Exception CreateUnsuccessfulResponseException(
        HttpResponseMessage httpResponse,
        string responseBody)
    {
        return CreateProblemDetailsResponseException(
            httpResponse,
            responseBody);
    }

    protected Exception CreateProblemDetailsResponseException(
        HttpResponseMessage httpResponse,
        string responseBody)
    {
        ProblemDetailsResponse problemDetailsResponse;

        if (responseBody.HasText())
        {
            if (responseBody.StartsWith("{"))
            {
                try
                {
                    problemDetailsResponse = JsonSerializationHelper.DeserializeFromJson<ProblemDetailsResponse>(responseBody);
                }
                catch (Exception ex)
                {
                    problemDetailsResponse = new ProblemDetailsResponse()
                    {
                        Title = "Unable to deserialize error response JSON.",
                        Detail = ex.Message,
                        Status = (int)httpResponse.StatusCode,
                        Extensions = new Dictionary<string, object?>()
                        {
                            { "ResponseBody", responseBody },
                        }
                    };
                }
            }
            else
            {
                problemDetailsResponse = new ProblemDetailsResponse()
                {
                    Title = "Error response is not JSON.",
                    Detail = responseBody,
                    Status = (int)httpResponse.StatusCode,
                };
            }
        }
        else
        {
            problemDetailsResponse = new ProblemDetailsResponse()
            {
                Title = "Error response is empty.",
                Detail = httpResponse.ReasonPhrase,
                Status = (int)httpResponse.StatusCode,
            };
        }

        return new ProblemDetailsException(problemDetailsResponse);
    }

    private string? ExecuteSerializeRequestBody<TRequest>(
        TRequest? request)
    {
        var requestType = typeof(TRequest);

        // If we're serializing a string request, just return the request body.
        if (requestType == typeof(string))
        {
            return request?.ToString();
        }
        // Else serialize to json.
        else
        {
            return SerializeRequestBody(request);
        }
    }

    protected virtual string? SerializeRequestBody<TRequest>(
        TRequest? request)
    {
        return JsonSerializationHelper.SerializeToJson(
            request,
            convertEnumsToStrings: this.SerializeEnumsToStrings);
    }

    private TResponse ExecuteDeserializeResponseBody<TResponse>(
        HttpResponseMessage httpResponse,
        string responseBody)
    {
        var responseType = typeof(TResponse);

        // If we're expecting a string response, just return the response body.
        if (responseType == typeof(string))
        {
            return (TResponse)Convert.ChangeType(responseBody, responseType);
        }
        // Else deserialize to the expected type.
        else
        {
            return DeserializeResponseBody<TResponse>(httpResponse, responseBody);
        }
    }

    protected virtual TResponse DeserializeResponseBody<TResponse>(
        HttpResponseMessage httpResponse,
        string responseBody)
    {
        return JsonSerializationHelper.DeserializeFromJson<TResponse>(
            responseBody,
            convertEnumsToStrings: this.DeserializeEnumsFromStrings);
    }
}
