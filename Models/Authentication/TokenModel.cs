namespace Models.Authentication
{
    public class TokenModel
    {
        public string JWT { get; set; }
        public DateTime JWTExpirationDate { get; set; }
    }
}
