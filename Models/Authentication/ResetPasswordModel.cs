using System.ComponentModel.DataAnnotations;

namespace Models.Authentication
{
    public class ResetPasswordModel
    {
        public string? Code { get; set; }

        public string Email { get; set; }
        public string Password { get; set; }

        [Compare("Password", ErrorMessage = "Password and ConfirmPassword Not Matching")]
        public string ConfirmPassword { get; set; }
    }
}
