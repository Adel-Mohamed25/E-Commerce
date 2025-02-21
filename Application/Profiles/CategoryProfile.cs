using AutoMapper;
using Domain.Entities;
using Models.Category;

namespace Application.Profiles
{
    public class CategoryProfile : Profile
    {
        public CategoryProfile()
        {
            Map();
        }

        private void Map()
        {
            CreateMap<CreateCategoryModel, Category>();
            CreateMap<Category, CreateCategoryModel>();

            CreateMap<CategoryModel, Category>();
            CreateMap<Category, CategoryModel>();
        }
    }
}
