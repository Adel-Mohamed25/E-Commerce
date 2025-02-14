using Domain.Entities.Identity;
using Domain.Enums;

namespace Domain.Entities
{
    public class Payment : BaseEntity
    {
        public string OrderId { get; set; } = null!;
        public string UserId { get; set; } = null!;
        public decimal Amount { get; set; }
        public PaymentMethod Method { get; set; }
        public PaymentStatus Status { get; set; }
        //public DateTime PaymentDate { get; set; }  
        public DateTime? ConfirmationDate { get; set; }

        public string? TransactionId { get; set; }

        public virtual Order Order { get; set; } = null!;
        public virtual User User { get; set; } = null!;
    }
}
