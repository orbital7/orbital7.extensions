using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Orbital7.Extensions.WebAPIClient
{
    public abstract class AuthenticatedAPIBase
    {
        protected string BaseAddress { get; set; }
        protected string AuthenticationToken { get; set; }

        protected AuthenticatedAPIBase(string baseAddress, string authenticationToken)
        {
            this.BaseAddress = baseAddress;
            this.AuthenticationToken = authenticationToken;
        }

        protected async Task<string> RetrieveJsonGetResponseStringAsync(
            string requestUri, 
            bool useAuthenticationBearer = true)
        {
            HttpResponseMessage response = await RetrieveJsonGetResponseAsync(requestUri, useAuthenticationBearer);
            return await response.Content.ReadAsStringAsync();
        }

        protected async Task<Stream> RetrieveJsonGetResponseStreamAsync(
            string requestUri, 
            bool useAuthenticationBearer = true)
        {
            HttpResponseMessage response = await RetrieveJsonGetResponseAsync(requestUri, useAuthenticationBearer);
            return await response.Content.ReadAsStreamAsync();
        }

        protected async Task<T> RetrieveJsonGetResponseObjectAsync<T>(
            string requestUri, 
            bool useAuthenticationBearer = true)
        {
            HttpResponseMessage response = await RetrieveJsonGetResponseAsync(requestUri, useAuthenticationBearer);
            return await response.Content.ReadAsAsync<T>();
        }

        protected async Task<string> RetrieveJsonPostResponseStringAsync(
            string requestUri, 
            IEnumerable<KeyValuePair<string, string>> contentItems, 
            bool useAuthenticationBearer = true)
        {
            HttpResponseMessage response = await RetrieveJsonPostResponseAsync(requestUri, contentItems, useAuthenticationBearer);
            return await response.Content.ReadAsStringAsync();
        }

        protected async Task<Stream> RetrieveJsonPostResponseStreamAsync(
            string requestUri, 
            IEnumerable<KeyValuePair<string, string>> contentItems,
            bool useAuthenticationBearer = true)
        {
            HttpResponseMessage response = await RetrieveJsonPostResponseAsync(requestUri, contentItems, useAuthenticationBearer);
            return await response.Content.ReadAsStreamAsync();
        }

        protected async Task<string> RetrieveJsonPostResponseObjectAsync(
            string requestUri, 
            object content = null, 
            bool useAuthenticationBearer = true)
        {
            HttpResponseMessage response = await RetrieveJsonPostResponseAsync(requestUri, content, useAuthenticationBearer);
            return await response.Content.ReadAsStringAsync();
        }

        protected async Task<T> RetrieveJsonPostResponseObjectAsync<T>(
            string requestUri,
            object content = null,
            bool useAuthenticationBearer = true)
        {
            HttpResponseMessage response = await RetrieveJsonPostResponseAsync(requestUri, content, useAuthenticationBearer);
            return await response.Content.ReadAsAsync<T>();
        }

        private async Task<HttpResponseMessage> RetrieveJsonGetResponseAsync(
            string requestUri, 
            bool useAuthenticationBearer = true)
        {
            HttpResponseMessage response = null;

            using (var client = CreateHttpClient(useAuthenticationBearer))
            {
                // Send the GET request.
                response = await client.GetAsync(requestUri);
                await ValidateResponseAsync(response);
            }

            return response;
        }

        private async Task<HttpResponseMessage> RetrieveJsonPostResponseAsync(
            string requestUri, 
            IEnumerable<KeyValuePair<string, string>> contentItems,
            bool useAuthenticationBearer = true)
        {
            HttpResponseMessage jsonResponse = null;

            using (var client = CreateHttpClient(useAuthenticationBearer))
            {
                // Get the content.
                FormUrlEncodedContent content = new FormUrlEncodedContent(contentItems);

                // Send the POST request.
                jsonResponse = await client.PostAsync(requestUri, content);
                await ValidateResponseAsync(jsonResponse);
            }

            return jsonResponse;
        }

        private async Task<HttpResponseMessage> RetrieveJsonPostResponseAsync(
            string requestUri, 
            object content,
            bool useAuthenticationBearer = true)
        {
            HttpResponseMessage jsonResponse = null;

            using (var client = CreateHttpClient(useAuthenticationBearer))
            {
                // Send the POST request.
                jsonResponse = await client.PostAsJsonAsync(requestUri, content);
                await ValidateResponseAsync(jsonResponse);
            }

            return jsonResponse;
        }

        private HttpClient CreateHttpClient(bool useAuthenticationBearer)
        {
            var client = new HttpClient
            {
                BaseAddress = new Uri(this.BaseAddress)
            };
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            if (useAuthenticationBearer && !string.IsNullOrEmpty(this.AuthenticationToken))
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", this.AuthenticationToken);

            return client;
        }

        private async Task<bool> ValidateResponseAsync(HttpResponseMessage response)
        {
            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            else
            {
                string content = await response.Content.ReadAsStringAsync();
                string description = content.FindFirstBetween("\"ExceptionMessage\":\"", "\\r\\n\",");
                if (string.IsNullOrEmpty(description))
                    description = content.FindFirstBetween("\"ExceptionMessage\":\"", "\",");
                if (string.IsNullOrEmpty(description))
                    description = content.FindFirstBetween("\"error_description\":\"", "\"");

                throw new Exception("Error " + Convert.ToInt32(response.StatusCode) + ": " + response.ReasonPhrase + ". " + 
                    description);
            }
        }
    }
}
