using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Storage;
using Windows.Web.Http;
using Windows.Web.Http.Headers;

namespace ProductHuntClient
{
    public sealed class PHClient
    {
        const string GrantType = "client_credentials";
        const string TokenRoute = "/v2/oauth/token";
        const string GraphQLRoute = "/v2/api/graphql";
        const string ContentType = "application/json";

        const string ClientIDString = "client_id";
        const string ClientSecretString = "client_secret";
        const string GrantTypeString = "grant_type";

        const string AccessTokenString = "access_token";

        HttpClient _client = new HttpClient();


        string _clientID;
        string _clientSecret;

        public PHClient(string clientID, string clientSecret)
        {
            _clientID = clientID;
            _clientSecret = clientSecret;
        }

        public IAsyncOperation<bool> AuthorizeAsync()
        {
            return AuthorizeAsyncTask().AsAsyncOperation();
        }

        private async Task<bool> AuthorizeAsyncTask()
        {
            bool wasAuthorized = false;
            _client.DefaultRequestHeaders.Accept.Add(HttpMediaTypeWithQualityHeaderValue.Parse("application/json"));
            _client.DefaultRequestHeaders.Host = new Windows.Networking.HostName("api.producthunt.com");

            var authorizationContent = CreateAuthContent();

            var response = await _client.PostAsync(new Uri("https://" +_client.DefaultRequestHeaders.Host + TokenRoute), authorizationContent);

            if (response.IsSuccessStatusCode)
            {
                wasAuthorized = true;
                StoreAccessToken(response.Content.ToString());
            }

            return wasAuthorized;
        }

        private void StoreAccessToken(string authResponseBody)
        {
            JObject tokenJson = JObject.Parse(authResponseBody);
            string accessToken = tokenJson.Value<string>(AccessTokenString);
            SaveTokenInLocalSettings(accessToken);
        }

        private void SaveTokenInLocalSettings(string accessToken)
        {
            var localSettings = ApplicationData.Current.LocalSettings;
            localSettings.Values[AccessTokenString] = accessToken;
            Debug.WriteLine($"Access Token saved as: {accessToken}");
        }

        private HttpStringContent CreateAuthContent()
        {
            JObject authJson = new JObject();
            authJson.Add(ClientIDString, JToken.FromObject(_clientID));
            authJson.Add(ClientSecretString, JToken.FromObject(_clientSecret));
            authJson.Add(GrantTypeString, JToken.FromObject(GrantType));
            
            return new HttpStringContent(authJson.ToString(), Windows.Storage.Streams.UnicodeEncoding.Utf8, ContentType);
        }


    }
}
