﻿namespace Models.Email
{
    public class EmailResponse
    {
        public bool IsConfirmed { get; set; }
        public string? Message { get; set; }
        public string Token { get; set; }
        public string? UserId { get; set; }
    }
}
