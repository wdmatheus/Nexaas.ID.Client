using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Nexaas.ID.Client.Entities;


namespace Nexaas.ID.Client
{
    public class NexaasID
    {
        private readonly string _clientId;

        private readonly string _clientSecret;

        private readonly string _redirectUri;

        private readonly HttpClient _httpClient;

        public readonly string BaseUri;

        private NexaasID(string clientId, string clientSecret, string redirectUri = null, string baseUri = null)
        {
            BaseUri = baseUri ?? "https://id.nexaas.com";
            _clientId = clientId;
            _clientSecret = clientSecret;
            _redirectUri = redirectUri;
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri(BaseUri)
            };
            ValidateClient();
        }

        public static NexaasID Sandbox(string clientId, string clientSecret, string redirectUri = null) =>
            new NexaasID(clientId, clientSecret, redirectUri, "https://sandbox.id.nexaas.com");

        public static NexaasID Production(string clientId, string clientSecret, string redirectUri = null) =>
            new NexaasID(clientId, clientSecret, redirectUri);

        private AuthenticationHeaderValue SetAuthenticationBearerAccessToken(string accessToken) =>
            new AuthenticationHeaderValue("Bearer", accessToken);

        private async Task<BaseResponse<T>> SendAsync<T>(HttpMethod method, string resource, string accessToken = null,
            object data = null)
        {
            using (var requestMessage = new HttpRequestMessage(method, resource))
            {
                var baseResponse = new BaseResponse<T>();

                if (!string.IsNullOrWhiteSpace(accessToken))
                {
                    requestMessage.Headers.Authorization =
                        SetAuthenticationBearerAccessToken(accessToken);
                }

                if (data != null)
                {
                    requestMessage.Content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8,
                        "application/json");
                }

                var response = await _httpClient.SendAsync(requestMessage);

                baseResponse.StatusCode = response.StatusCode;

                baseResponse.Content = await response.Content.ReadAsStringAsync();

                if (baseResponse.StatusCode == HttpStatusCode.OK)
                    baseResponse.Data = JsonConvert.DeserializeObject<T>(baseResponse.Content);

                return baseResponse;
            }
        }

        public string GetAuthorizeUrl(string redirectUri = null)
        {
            if (!string.IsNullOrWhiteSpace(redirectUri) && !Validations.IsUrl(redirectUri))
                throw new NexaasIDException(Messages.InvalidRedirectUri);
            
            return BaseUri
                .AddPath("oauth")
                .AddPath("authorize")
                .AddQueryStringParameter("client_id", _clientId)
                .AddQueryStringParameter("response_type", "code")
                .AddQueryStringParameter("redirect_uri", redirectUri ?? _redirectUri)
                .AddQueryStringParameter("scope", "profile+invite");
        }

        public async Task<BaseResponse<OauthTokenResponse>> GetAuthorizationToken(string code,
            string redirectUri = null)
        {
            if (string.IsNullOrWhiteSpace(code))
                throw new NexaasIDException(Messages.EmptyCode);

            if (!string.IsNullOrWhiteSpace(redirectUri) && !Validations.IsUrl(redirectUri))
                throw new NexaasIDException(Messages.InvalidRedirectUri);

            return await SendAsync<OauthTokenResponse>(HttpMethod.Post, "oauth/token", null, new OauthTokenRequest(
                _clientId,
                _clientSecret,
                code,
                redirectUri ?? _redirectUri
            ));
        }


        public async Task<BaseResponse<Profile>> GetProfile(string accessToken)
        {
            ValidateAccessToken(accessToken);
            return await SendAsync<Profile>(HttpMethod.Get, "api/v1/profile", accessToken);
        }

        public async Task<BaseResponse<Profile>> GetProfile(OauthTokenResponse oauthTokenResponse) =>
            await GetProfile(oauthTokenResponse.AccessToken);

        public async Task<BaseResponse<ProfessionalInfo>> GetProfessionalInfo(string accessToken)
        {
            ValidateAccessToken(accessToken);
            return await SendAsync<ProfessionalInfo>(HttpMethod.Get, "api/v1/profile/professional_info", accessToken);
        }

        public async Task<BaseResponse<ProfessionalInfo>> GetProfessionalInfo(OauthTokenResponse oauthTokenResponse) =>
            await GetProfessionalInfo(oauthTokenResponse.AccessToken);

        public async Task<BaseResponse<Contacts>> GetContacts(string accessToken)
        {
            ValidateAccessToken(accessToken);
            return await SendAsync<Contacts>(HttpMethod.Get, "api/v1/profile/contacts", accessToken);
        }

        public async Task<BaseResponse<Contacts>> GetContacts(OauthTokenResponse oauthTokenResponse) =>
            await GetContacts(oauthTokenResponse.AccessToken);

        public async Task<BaseResponse<Emails>> GetEmails(string accessToken)
        {
            ValidateAccessToken(accessToken);
            return await SendAsync<Emails>(HttpMethod.Get, "api/v1/profile/emails", accessToken);
        }


        public async Task<BaseResponse<Emails>> GetEmails(OauthTokenResponse oauthTokenResponse) =>
            await GetEmails(oauthTokenResponse.AccessToken);

        public async Task<BaseResponse<ApplicationInvitiationResponse>> InviteToApplication(
            ApplicationInvitiationRequest applicationInvitiationRequest)
        {
            if (applicationInvitiationRequest == null)
                throw new NexaasIDException(Messages.NullApplicationInvitiationRequest);

            if (string.IsNullOrWhiteSpace(applicationInvitiationRequest.Email))
                throw new NexaasIDException(Messages.EmptyApplicationInvitiationRequestEmail);

            if (!Validations.IsEmail(applicationInvitiationRequest.Email))
                throw new NexaasIDException(Messages.InvalidEmail);

            ValidateAccessToken(applicationInvitiationRequest.AccessToken);

            return await SendAsync<ApplicationInvitiationResponse>(HttpMethod.Post, "api/v1/sign_up",
                applicationInvitiationRequest.AccessToken, new
                {
                    invited = applicationInvitiationRequest.Email
                });
        }

        public async Task<BaseResponse<OauthTokenResponse>> GetClientAuthorizationToken(string scope)
        {
            if (string.IsNullOrWhiteSpace(scope))
            {
                throw new NexaasIDException(Messages.EmptyScope);
            }

            return await SendAsync<OauthTokenResponse>(HttpMethod.Post, "oauth/token", data: new OauthTokenRequest(
                _clientId,
                _clientSecret,
                null,
                _redirectUri,
                "client_credentials",
                scope));
        }

        private void ValidateClient()
        {
            if (string.IsNullOrWhiteSpace(_clientId))
                throw new NexaasIDException(Messages.EmptyClientId);

            if (string.IsNullOrWhiteSpace(_clientSecret))
                throw new NexaasIDException(Messages.EmptyClientSecret);
            
            if (!string.IsNullOrWhiteSpace(_redirectUri) && !Validations.IsUrl(_redirectUri))
                throw new NexaasIDException(Messages.InvalidRedirectUri);
        }

        private void ValidateAccessToken(string accessToken)
        {
            if (string.IsNullOrWhiteSpace(accessToken))
                throw new NexaasIDException(Messages.EmptyAccessToken);
        }
    }
}