using AutoMapper;
using Domain.Entities.Identity;
using Models.Role;

namespace Application.Profiles
{
    public class RoleProfile : Profile
    {
        public RoleProfile()
        {
            Map();
        }

        private void Map()
        {
            CreateMap<CreateRoleModel, Role>();
            CreateMap<Role, CreateRoleModel>();

            CreateMap<RoleModel, Role>();
            CreateMap<Role, RoleModel>();
        }
    }
}
