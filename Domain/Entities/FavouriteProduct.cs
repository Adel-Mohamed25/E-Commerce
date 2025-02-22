using Domain.Commons;
using Domain.Entities.Identity;

namespace Domain.Entities
{
    public class FavouriteProduct : BaseEntity
    {
        public string UserId { get; set; }

        public virtual User User { get; set; }

        public virtual List<FavouriteItem> FavoriteItems { get; set; } = new List<FavouriteItem>();

    }
}
