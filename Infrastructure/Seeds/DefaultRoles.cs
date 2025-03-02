using Domain.Entities.Identity;
using Infrastructure.Constants;
using Infrastructure.UnitOfWorks;

namespace Infrastructure.Seeders
{
    public static class DefaultRoles
    {
        public static async Task SeedAsync(IUnitOfWork unitOfWork)
        {
            if (!await unitOfWork.Roles.IsExistAsync())
            {
                await unitOfWork.Roles.RoleManager.CreateAsync(new Role { Name = Roles.SuperAdmin.ToString() });
                await unitOfWork.Roles.RoleManager.CreateAsync(new Role { Name = Roles.Admin.ToString() });
                await unitOfWork.Roles.RoleManager.CreateAsync(new Role { Name = Roles.Basic.ToString() });
            }
        }
    }
}
