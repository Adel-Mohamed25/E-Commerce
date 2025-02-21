using AutoMapper;
using Domain.Entities.Identity;
using Models.User;

namespace Application.Profiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            Map();
        }

        private void Map()
        {
            CreateMap<CreateUserModel, User>();
            CreateMap<User, CreateUserModel>();
        }
    }
}
