using Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace Models.User
{
    public class PostUserModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        [EmailAddress]
        public string Email { get; set; }
        public string? Address { get; set; }
        public int? Age { get; set; }

        public string PhoneNumber { get; set; }
        public string? ProfilePictureUrl { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public GenderType Gender { get; set; }

        public string Password { get; set; }

        [Compare("Password", ErrorMessage = "Password Not Matching")]
        public string ConfirmPassword { get; set; }
    }
}
