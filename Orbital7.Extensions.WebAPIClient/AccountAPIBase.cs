using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;

namespace Orbital7.Extensions.WebAPIClient
{
    public abstract class AccountAPIBase : AuthenticatedAPIBase
    {
        public AccountAPIBase(string serviceUri, string authenticationToken)
            : base(serviceUri, authenticationToken) { }

        public async Task<TokenResponse> GetAuthenticationTokenAsync(string username, string password)
        {
            // Obtain the response.
            Stream jsonResponse = await base.RetrieveJsonPostResponseStreamAsync("Token", new List<KeyValuePair<string, string>>() 
            {
                new KeyValuePair<string, string>("grant_type", "password"), 
                new KeyValuePair<string, string>("username", username), 
                new KeyValuePair<string, string>("password", password), 
            }, false);

            // Parse the response.
            var serializer = new DataContractJsonSerializer(typeof(TokenResponse));
            TokenResponse response = serializer.ReadObject(jsonResponse) as TokenResponse;

            // Set the authentication token for this service.
            this.AuthenticationToken = response.Token;

            return response;
        }
    }
}
