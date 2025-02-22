using Domain.Commons;

namespace Domain.Entities
{
    public class FavouriteItem : BaseEntity
    {
        public string FavoriteProductId { get; set; }

        public string ProductId { get; set; }

        public string ProductName { get; set; }

        public virtual FavouriteProduct FavouriteProduct { get; set; }

        public virtual Product Product { get; set; }
    }
}
