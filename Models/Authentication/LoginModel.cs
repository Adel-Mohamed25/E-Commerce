using System.ComponentModel.DataAnnotations;

namespace Models.Authentication
{
    public class LoginModel
    {
        [EmailAddress]
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
