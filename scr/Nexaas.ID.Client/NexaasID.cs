using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Nexaas.ID.Client.Entities;

namespace Nexaas.ID.Client
{
    public class NexaasID 
    {
        private readonly string _clientId;

        private readonly string _clientSecret;

        private readonly Uri _baseUri;

        private readonly string _redirectUri;

        private readonly HttpClient _httpClient;

        public NexaasID(string clientId, string clientSecret, string redirectUri = null, string baseUri = null)
        {
            _baseUri = new Uri(baseUri ?? "https://id.nexaas.com");
            _clientId = clientId;
            _clientSecret = clientSecret;
            _redirectUri = redirectUri;
            _httpClient = new HttpClient()
            {
                BaseAddress = _baseUri
            };
        }

        public static NexaasID Sandbox(string clientId, string clientSecret, string redirectUri = null) =>
            new NexaasID(clientId, clientSecret, redirectUri, "https://sandbox.id.nexaas.com");
        
        public static NexaasID Production(string clientId, string clientSecret, string redirectUri = null) =>
            new NexaasID(clientId, clientSecret, redirectUri);

        public string GetAuthorizeUrl(string redirectUri = null)
        {
            return _baseUri.ToString()
                .AddPath("oauth")
                .AddPath("authorize")
                .AddQueryStringParameter("client_id", _clientId)
                .AddQueryStringParameter("response_type", "code")
                .AddQueryStringParameter("redirect_uri", redirectUri ?? _redirectUri)
                .AddQueryStringParameter("scope", "profile");
        }

        public async Task<OauthTokenResponse> GetAuthorizationToken(string code, string redirectUri = null)
        {
            var data = new OauthTokenRequest(_clientId, _clientSecret, code, redirectUri ?? _redirectUri);

            var content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(data), Encoding.UTF8,
                "application/json");

            var response = await _httpClient.PostAsync("oauth/token", content);

            var oauthTokenResponse =
                Newtonsoft.Json.JsonConvert.DeserializeObject<OauthTokenResponse>(
                    await response.Content.ReadAsStringAsync());

            return oauthTokenResponse;
        }

        public async Task<Profile> GetProfile(string accessToken)
        {
            using( var requestMessage = new HttpRequestMessage(HttpMethod.Get, "api/v1/profile"))
            {
                requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
              
                var response = await _httpClient.SendAsync(requestMessage);
                 
                var profile = Newtonsoft.Json.JsonConvert.DeserializeObject<Profile>(
                    await response.Content.ReadAsStringAsync());

                return profile;
            }
        }

        public async Task<Profile> GetProfile(OauthTokenResponse oauthTokenResponse) =>
            await GetProfile(oauthTokenResponse.AccessToken);

       
    }
}