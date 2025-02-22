using Contracts.Repositories.IdentityRepositories;
using Domain.Entities.Identity;
using Infrastructure.Caching;
using Microsoft.AspNetCore.Identity;
using Persistence.Context;

namespace Infrastructure.Repositories.IdentityRepositories
{
    public class RoleRepository : GenericRepository<Role>, IRoleRepository
    {
        public RoleRepository(RoleManager<Role> roleManager,
            IApplicationDbContext context,
            IRedisCacheService cache) : base(context, cache)
        {
            RoleManager = roleManager;
        }
        public RoleManager<Role> RoleManager { get; }
    }
}
