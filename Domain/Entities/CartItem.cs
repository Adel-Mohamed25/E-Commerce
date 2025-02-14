namespace Domain.Entities
{
    public class CartItem : BaseEntity
    {
        public string CartId { get; set; } = null!;
        public string ProductId { get; set; } = null!;
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice => Quantity * UnitPrice;

        public virtual Cart Cart { get; set; } = null!;
        public virtual Product Product { get; set; } = null!;
    }
}
