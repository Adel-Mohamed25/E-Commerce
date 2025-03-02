using Domain.Entities.Identity;
using Infrastructure.Constants;
using Infrastructure.UnitOfWorks;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace Infrastructure.Seeders
{
    public static class DefaultUsers
    {
        public static async Task SeedBasicUserAsync(IUnitOfWork unitOfWork)
        {
            var defaultBasicUser = new User
            {
                FirstName = "Adel",
                LastName = "Mohamed",
                UserName = "adel2852003adel@gmail.com",
                Email = "adel2852003adel@gmail.com",
                EmailConfirmed = true,
            };

            if (!await unitOfWork.Users.IsExistAsync(bu => bu.Email == defaultBasicUser.Email))
            {
                await unitOfWork.Users.UserManager.CreateAsync(defaultBasicUser, "1Q2w3e4@");
                await unitOfWork.Users.UserManager.AddToRoleAsync(defaultBasicUser, Roles.Basic.ToString());
            }
        }

        public static async Task SeedAdminUserAsync(IUnitOfWork unitOfWork)
        {
            var defaultAdminUser = new User
            {
                FirstName = "Adel",
                LastName = "Mohamed",
                UserName = "adel2752003adel@gmail.com",
                Email = "adel2752003adel@gmail.com",
                EmailConfirmed = true,
            };

            if (!await unitOfWork.Users.IsExistAsync(bu => bu.Email == defaultAdminUser.Email))
            {
                await unitOfWork.Users.UserManager.CreateAsync(defaultAdminUser, "1Q2w3e4@");
                await unitOfWork.Users.UserManager.AddToRoleAsync(defaultAdminUser, Roles.Basic.ToString());
                await unitOfWork.Users.UserManager.AddToRoleAsync(defaultAdminUser, Roles.Admin.ToString());
            }
        }

        public static async Task SeedSuperAdminUserAsync(IUnitOfWork unitOfWork)
        {
            var defaultSuperAdminUser = new User
            {
                FirstName = "Adel",
                LastName = "Mohamed",
                UserName = "adel3052003adel@gmail.com",
                Email = "adel3052003adel@gmail.com",
                EmailConfirmed = true,
            };

            if (!await unitOfWork.Users.IsExistAsync(bu => bu.Email == defaultSuperAdminUser.Email))
            {
                await unitOfWork.Users.UserManager.CreateAsync(defaultSuperAdminUser, "1Q2w3e4@");
                await unitOfWork.Users.UserManager.AddToRoleAsync(defaultSuperAdminUser, Roles.Basic.ToString());
                await unitOfWork.Users.UserManager.AddToRoleAsync(defaultSuperAdminUser, Roles.Admin.ToString());
                await unitOfWork.Users.UserManager.AddToRoleAsync(defaultSuperAdminUser, Roles.SuperAdmin.ToString());
            }
            await unitOfWork.Roles.RoleManager.SeedClaimsForSuperAdminAsync();
        }

        private static async Task SeedClaimsForSuperAdminAsync(this RoleManager<Role> roleManager)
        {
            var superAdminRole = await roleManager.FindByNameAsync(Roles.SuperAdmin.ToString());
            await roleManager.AddPermissionClaimsAsync(superAdminRole!, "Categories");
        }

        public static async Task AddPermissionClaimsAsync(this RoleManager<Role> roleManager, Role role, string module)
        {
            var allClaims = await roleManager.GetClaimsAsync(role);
            var allPermissions = Permissions.GeneratePermissionsForModule(module);
            foreach (var permission in allPermissions)
            {
                if (!allClaims.Any(a => a.Type == "Permission" && a.Value == permission))
                {
                    await roleManager.AddClaimAsync(role, new Claim("Permission", permission));
                }
            }
        }
    }
}
