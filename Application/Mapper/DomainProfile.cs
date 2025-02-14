using AutoMapper;
using Domain.Entities;
using Models.Category;

namespace Application.Mapper
{
    public class DomainProfile : Profile
    {
        public DomainProfile()
        {
            Map();
        }

        private void Map()
        {
            CreateMap<PostCategoryModel, Category>();
            CreateMap<Category, PostCategoryModel>();

            CreateMap<CategoryModel, Category>();
            CreateMap<Category, CategoryModel>();



        }
    }
}
