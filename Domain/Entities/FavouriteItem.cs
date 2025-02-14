namespace Domain.Entities
{
    public class FavouriteItem : BaseEntity
    {
        public string FavoriteProductId { get; set; } = null!;

        public string ProductId { get; set; } = null!;

        public string ProductName { get; set; }

        public virtual FavouriteProduct FavouriteProduct { get; set; } = null!;

        public virtual Product Product { get; set; } = null!;
    }
}
