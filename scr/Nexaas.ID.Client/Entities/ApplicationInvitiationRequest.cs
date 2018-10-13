namespace Nexaas.ID.Client.Entities
{
    public class ApplicationInvitiationRequest
    {
        public ApplicationInvitiationRequest(string email, string accessToken)
        {
            Email = email;
            AccessToken = accessToken;
        }

        public string Email { get; }

        public string AccessToken { get;  }
    }
}