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
        static ApplicationDataContainer _localSettings = ApplicationData.Current.LocalSettings;


        string _clientID;
        string _clientSecret;

        public bool TokenExists { get; set; }

        public PHClient(string clientID, string clientSecret)
        {
            _clientID = clientID;
            _clientSecret = clientSecret;            
            SetDefaults();
        }

        private void SetDefaults()
        {
            _client.DefaultRequestHeaders.Accept.Add(HttpMediaTypeWithQualityHeaderValue.Parse("application/json"));
            _client.DefaultRequestHeaders.Host = new Windows.Networking.HostName("api.producthunt.com");

            if (TokenExists = CheckIfTokenExists()){
                LoadToken();
            }
        }

        public void SetTokenAsAuthField(string token)
        {
            _client.DefaultRequestHeaders.Authorization = HttpCredentialsHeaderValue.Parse($"Bearer {token}");
        }

        public IAsyncOperation<bool> AuthorizeAsync()
        {
            return AuthorizeAsyncTask().AsAsyncOperation();
        }

        private async Task<bool> AuthorizeAsyncTask()
        {
            bool wasAuthorized = false;
           

            var authorizationContent = CreateAuthContent();

            var response = await _client.PostAsync(new Uri("https://" + _client.DefaultRequestHeaders.Host + TokenRoute), authorizationContent);

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
            SetTokenAsAuthField(accessToken);
        }

        private void SaveTokenInLocalSettings(string accessToken)
        {
            _localSettings.Values[AccessTokenString] = accessToken;
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


        public IAsyncOperation<IList<PHPost>> GetTopPostsAsync()
        {
            return GetTopPostsAsyncTask().AsAsyncOperation();
        }

        private async Task<IList<PHPost>> GetTopPostsAsyncTask()
        {
            string query =
               @"query {
  posts(first: 3) {
    edges {
      node {
        name
        description
        url
        thumbnail {
          url(width: 200, height: 200)
        }
      }
    }
  }
}";
            JObject queryObject = new JObject();

            queryObject.Add("query", JToken.FromObject(query));
           

            var response = await QueryForPosts(queryObject.ToString());
            Debug.WriteLine(response);
            IList<PHPost> parsedPosts = ParseResponseForPosts(response);
            Debug.WriteLine($"Top posts: {parsedPosts}");
            return parsedPosts;
        }

        private IList<PHPost> ParseResponseForPosts(string response)
        {
            JObject obj = JObject.Parse(response);
            JArray edges = (JArray)obj["data"]["posts"]["edges"];

            List<PHPost> postsToReturn = new List<PHPost>();

            for (int i = 0; i < edges.Count; i++)
            {
                Debug.WriteLine($"Edge {i}: {edges[i].ToString()}");
                var node = JsonConvert.DeserializeObject<Node>(edges[i].ToString());
                postsToReturn.Add(node.Post);
            }

            for (int i = 0; i < postsToReturn.Count; i++)
            {
                Debug.WriteLine(postsToReturn[i].Name);
            }
            return postsToReturn;

        }

        private async Task<string> QueryForPosts(string query)
        {
            string postsResponse = "";

           var response = await _client.PostAsync(new Uri("https://" + _client.DefaultRequestHeaders.Host + GraphQLRoute), 
               new HttpStringContent(query, Windows.Storage.Streams.UnicodeEncoding.Utf8, ContentType));

            if (response.IsSuccessStatusCode)
            {
                postsResponse = response.Content.ToString();
            }
            return postsResponse;
        }

        private void LoadToken()
        {
            string token = (string)_localSettings.Values[AccessTokenString];
            SetTokenAsAuthField(token);
        }

        public static bool CheckIfTokenExists()
        {
            return _localSettings.Values[AccessTokenString] != null;
        }
    }
}
