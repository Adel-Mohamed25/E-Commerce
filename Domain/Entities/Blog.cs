using Domain.Enums;

namespace Domain.Entities
{
    public class Blog : BaseEntity
    {
        public BlogType Type { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Author { get; set; }
        public string? AuthorImageUrl { get; set; }
        public string? ContentUrl { get; set; }
    }
}
