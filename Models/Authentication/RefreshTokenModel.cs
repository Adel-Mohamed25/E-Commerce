﻿namespace Models.Authentication
{
    public class RefreshTokenModel
    {
        public string RefreshToken { get; set; }
        public DateTime RefreshTokenExpirationDate { get; set; }
    }
}
