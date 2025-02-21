namespace Models.Category
{
    public class CreateCategoryModel
    {
        public string Name { get; set; } = null!;
        public string? ImgUrl { get; set; }
        public DateTime? CreatedDate { get; set; }

    }
}
