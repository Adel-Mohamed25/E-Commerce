using Domain.Commons;

namespace Domain.Entities
{
    public class OrderItem : BaseEntity
    {
        public string OrderId { get; set; }
        public string ProductId { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice => Quantity * UnitPrice;

        public virtual Order Order { get; set; }
        public virtual Product Product { get; set; }
    }
}
