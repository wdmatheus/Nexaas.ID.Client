namespace Nexaas.ID.Client
{
    public class Messages
    {
        public const string EmptyClientId = "Client id can't be null or empty";
        public const string EmptyClientSecret = "Client secret can't be null or empty";
        public const string InvalidRedirectUri = "Redirect uri is invalid";
        public const string InvalidEmail = "Email is invalid";
        public const string EmptyCode = "Code can't be null or empty";
        public const string EmptyAccessToken = "Access token can't be null or empty";
        public const string EmptyScope = "Scope can't be null or empty. Possible values profile and invite";
        public const string NullApplicationInvitiationRequest = "ApplicationInvitiationRequest can't be null";
        public const string EmptyApplicationInvitiationRequestEmail = "E-mail can't be null or empty";
    }
}