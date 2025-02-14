using Domain.Entities.Identity;

namespace Domain.Entities
{
    public class FavouriteProduct : BaseEntity
    {
        public string UserId { get; set; } = null!;

        public virtual User User { get; set; } = null!;

        public virtual List<FavouriteItem> FavoriteItems { get; set; } = null!;

    }
}
