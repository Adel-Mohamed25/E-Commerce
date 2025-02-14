namespace Models.Category
{
    public class CategoryModel
    {
        public string Id { get; set; } = null!;
        public string Name { get; set; }
        public string? ImgUrl { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }

    }
}
