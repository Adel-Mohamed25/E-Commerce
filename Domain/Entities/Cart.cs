using Domain.Entities.Comman;
using Domain.Entities.Identity;

namespace Domain.Entities
{
    public class Cart : BaseEntity
    {
        public string UserId { get; set; }

        public decimal TotalAmount => CartItems?.Sum(item => item.TotalPrice) ?? 0;

        public virtual User User { get; set; }
        public virtual List<CartItem> CartItems { get; set; } = new List<CartItem>();

    }
}
