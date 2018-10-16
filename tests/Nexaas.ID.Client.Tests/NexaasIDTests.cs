using System;
using Nexaas.ID.Client.Entities;
using Xunit;

namespace Nexaas.ID.Client.Tests
{
    public class NexaasIDTests
    {
        private NexaasID NexaasIdInstance = NexaasID.Production("client id", "client secret", "http://localhost:8080");

        [Fact(DisplayName = "Can't instantiate with empty client id")]
        public void CantInstantiateWithEmptyClientId()
        {
            Exception exception =
                Assert.Throws<NexaasIDException>(() => NexaasID.Production(string.Empty, "client secret"));

            Assert.Equal(exception.Message, Messages.EmptyClientId);
        }

        [Fact(DisplayName = "Can't instantiate with empty client secret")]
        public void CantInstantiateWithEmptyClientSecret()
        {
            Exception exception =
                Assert.Throws<NexaasIDException>(() => NexaasID.Production("client id", string.Empty));

            Assert.Equal(exception.Message, Messages.EmptyClientSecret);
        }

        [Fact(DisplayName = "Can't instantiate with invalid redirect uri")]
        public void CantInstantiateWithInvalidRedirectUril()
        {
            Exception exception =
                Assert.Throws<NexaasIDException>(() =>
                    NexaasID.Production("client id", "client secret", "invalid_uri"));

            Assert.Equal(exception.Message, Messages.InvalidRedirectUri);
        }

        [Fact(DisplayName = "Can't get authorization token with empty code")]
        public void CantGetAuthorizationTokenWithEmptyCode()
        {
            Exception exception =
                Assert.ThrowsAsync<NexaasIDException>(() => NexaasIdInstance.GetAuthorizationToken(string.Empty))
                    .Result;

            Assert.Equal(exception.Message, Messages.EmptyCode);
        }

        [Fact(DisplayName = "Can't get authorization token with invalid redirect uri")]
        public void CantGetAuthorizationTokenWithInvalidRedirectUri()
        {
            Exception exception =
                Assert.ThrowsAsync<NexaasIDException>(() =>
                        NexaasIdInstance.GetAuthorizationToken("code", "invalid uri"))
                    .Result;

            Assert.Equal(exception.Message, Messages.InvalidRedirectUri);
        }

        [Fact(DisplayName = "Can't get profile with access token")]
        public void CantGetProfileWithEmptyAccessToken()
        {
            Exception exception =
                Assert.ThrowsAsync<NexaasIDException>(() => NexaasIdInstance.GetProfessionalInfo(string.Empty))
                    .Result;

            Assert.Equal(exception.Message, Messages.EmptyAccessToken);
        }

        [Fact(DisplayName = "Can't get professional info with access token")]
        public void CantGetProfessionalInfoWithEmptyAccessToken()
        {
            Exception exception =
                Assert.ThrowsAsync<NexaasIDException>(() => NexaasIdInstance.GetProfessionalInfo(string.Empty))
                    .Result;

            Assert.Equal(exception.Message, Messages.EmptyAccessToken);
        }

        [Fact(DisplayName = "Can't get contacts info with access token")]
        public void CantGetContactsWithEmptyAccessToken()
        {
            Exception exception =
                Assert.ThrowsAsync<NexaasIDException>(() => NexaasIdInstance.GetContacts(string.Empty))
                    .Result;

            Assert.Equal(exception.Message, Messages.EmptyAccessToken);
        }

        [Fact(DisplayName = "Can't get emails info with access token")]
        public void CantGetEmailsWithEmptyAccessToken()
        {
            Exception exception =
                Assert.ThrowsAsync<NexaasIDException>(() => NexaasIdInstance.GetEmails(string.Empty))
                    .Result;

            Assert.Equal(exception.Message, Messages.EmptyAccessToken);
        }

        [Fact(DisplayName = "Can't invite to application with null ApplicationInvitiationResponse")]
        public void CantInviteToApplicationWithNullApplicationInvitiationResponse()
        {
            Exception exception =
                Assert.ThrowsAsync<NexaasIDException>(() => NexaasIdInstance.InviteToApplication(null))
                    .Result;

            Assert.Equal(exception.Message, Messages.NullApplicationInvitiationRequest);
        }

        [Fact(DisplayName = "Can't invite to application with null with empty access token")]
        public void CantInviteToApplicationWithEmptyAccessToken()
        {
            Exception exception =
                Assert.ThrowsAsync<NexaasIDException>(() =>
                        NexaasIdInstance.InviteToApplication(
                            new ApplicationInvitiationRequest("test@gmail.com", string.Empty)))
                    .Result;

            Assert.Equal(exception.Message, Messages.EmptyAccessToken);
        }
        
        [Fact(DisplayName = "Can't invite to application with null with empty email")]
        public void CantInviteToApplicationWithEmptyEmail()
        {
            Exception exception =
                Assert.ThrowsAsync<NexaasIDException>(() =>
                        NexaasIdInstance.InviteToApplication(
                            new ApplicationInvitiationRequest(string.Empty, "access token")))
                    .Result;

            Assert.Equal(exception.Message, Messages.EmptyApplicationInvitiationRequestEmail);
        }
        
        [Fact(DisplayName = "Can't invite to application with null with invalid email")]
        public void CantInviteToApplicationWithInvalidEmail()
        {
            Exception exception =
                Assert.ThrowsAsync<NexaasIDException>(() =>
                        NexaasIdInstance.InviteToApplication(
                            new ApplicationInvitiationRequest("invalid_email", "access token")))
                    .Result;

            Assert.Equal(exception.Message, Messages.InvalidEmail);
        }
    }
}