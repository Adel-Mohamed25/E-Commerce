using Domain.Commons;
using Domain.Entities.Identity;

namespace Domain.Entities
{
    public class Review : BaseEntity
    {
        public int Rating { get; set; }
        public string ReviewText { get; set; }
        public string UserId { get; set; }
        public User User { get; set; }
        public string ProductId { get; set; }
        public Product Product { get; set; }
        public List<Reply> Replies { get; set; } = new List<Reply>();
    }
}
