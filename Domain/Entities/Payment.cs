using Domain.Commons;
using Domain.Entities.Identity;
using Domain.Enums;

namespace Domain.Entities
{
    public class Payment : BaseEntity
    {
        public string OrderId { get; set; }
        public string UserId { get; set; }
        public decimal Amount { get; set; }
        public PaymentMethod Method { get; set; }
        public PaymentStatus Status { get; set; }
        //public DateTime PaymentDate { get; set; }  
        public DateTime? ConfirmationDate { get; set; }

        public string? TransactionId { get; set; }

        public virtual Order Order { get; set; }
        public virtual User User { get; set; }
    }
}
