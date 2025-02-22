using Domain.Commons;

namespace Domain.Entities
{
    public class Product : BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string SKU { get; set; }  //(SKU)Stock Keeping Unit
        public int StockQuantity { get; set; }
        public bool IsAvailable { get; set; }
        public bool TopSelling { get; set; }
        public string ImageURL { get; set; }
        public string CreatedBy { get; set; }
        public string Brand { get; set; }
        public string Dimensions { get; set; }
        public string Weight { get; set; }
        public string Color { get; set; }
        public string Material { get; set; }
        public decimal Discount { get; set; }
        public decimal FinalPrice { get; set; }
        public string ShippingWeight { get; set; }
        public decimal ShippingCost { get; set; }
        public int Rating { get; set; }
        public List<Review> ReviewsList { get; set; } = new List<Review>();
        public int NumberOfStarts { get; set; }
        public string CategoryId { get; set; }
        public Category Category { get; set; }
        public List<CartItem> CartItemsList { get; set; } = new List<CartItem>();
        public List<FavouriteItem> FavouriteItems { get; set; } = new List<FavouriteItem>();
        public List<OrderItem> OrderItemsList { get; set; } = new List<OrderItem>();

    }
}
