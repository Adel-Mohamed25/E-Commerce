namespace Models.Authentication
{
    public class ExternalAuthRequest
    {
        public string Provider { get; set; } // "Google" or "Facebook"
        public string ProviderKey { get; set; } // The ID token or access token from provider
        public string Email { get; set; }
    }
}
