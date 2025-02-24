using Contracts.Abstractions.IdentityRepositories;
using Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Persistence.Context;

namespace Infrastructure.Repositories.IdentityRepositories
{
    public class RoleRepository : GenericRepository<Role>, IRoleRepository
    {
        public RoleRepository(RoleManager<Role> roleManager,
            IApplicationDbContext context) : base(context)
        {
            RoleManager = roleManager;
        }
        public RoleManager<Role> RoleManager { get; }
    }
}
