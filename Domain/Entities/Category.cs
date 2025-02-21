using Domain.Entities.Comman;

namespace Domain.Entities
{
    public class Category : BaseEntity
    {
        public string Name { get; set; }
        public string? ImgUrl { get; set; }
        public List<Product> Products { get; set; }
    }
}
