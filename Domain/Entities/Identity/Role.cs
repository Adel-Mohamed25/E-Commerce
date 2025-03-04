using Microsoft.AspNetCore.Identity;

namespace Domain.Entities.Identity
{
    public class Role : IdentityRole<string>
    {
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }
}
