using Newtonsoft.Json;

namespace Nexaas.ID.Client.Entities
{
    public class OauthTokenRequest
    {
        public OauthTokenRequest()
        {
            
        }
        
        public OauthTokenRequest(string clientId, string clientSecret, string code, string redirectUri,
            string grantType = "authorization_code", string scope = null)
        {
            ClientId = clientId;
            ClientSecret = clientSecret;
            Code = code;
            RedirectUri = redirectUri;
            GrantType = grantType;
            Scope = scope;
        }

        [JsonProperty("client_id")]
        public string ClientId { get; set; }

        [JsonProperty("client_secret")]
        public string ClientSecret { get; set; }

        [JsonProperty("code")]
        public string Code { get; set; }

        [JsonProperty("redirect_uri")]
        public string RedirectUri { get; set; }

        [JsonProperty("grant_type")]
        public string GrantType { get; set; }
        
        [JsonProperty("scope")]
        public string Scope { get; set; }
    }
}