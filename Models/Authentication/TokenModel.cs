namespace Models.Authentication
{
    public class TokenModel
    {
        public string Token { get; set; }
        public DateTime TokenExpirationDate { get; set; }
    }
}
