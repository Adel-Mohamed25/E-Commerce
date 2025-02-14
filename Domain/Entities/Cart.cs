using Domain.Entities.Identity;

namespace Domain.Entities
{
    public class Cart : BaseEntity
    {
        public string UserId { get; set; } = null!;

        public decimal TotalAmount => CartItems?.Sum(item => item.TotalPrice) ?? 0;

        public virtual User User { get; set; } = null!;
        public virtual List<CartItem> CartItems { get; set; } = null!;

    }
}
