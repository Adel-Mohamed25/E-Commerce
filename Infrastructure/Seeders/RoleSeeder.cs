using Domain.Entities.Identity;
using Infrastructure.UnitOfWorks;

namespace Infrastructure.Seeders
{
    public static class RoleSeeder
    {
        public static async Task SeedRoleAsync(IUnitOfWork unitOfWork)
        {
            string[] roleNames = { "Admin", "Customer", "Seller", "Delivery" };
            foreach (var roleName in roleNames)
            {
                if (!await unitOfWork.Roles.RoleManager.RoleExistsAsync(roleName))
                {
                    await unitOfWork.Roles.RoleManager.CreateAsync(new Role() { Name = roleName });
                }
            }
        }
    }
}
