using Contracts.Contracts.IIdentityRepository;
using Domain.Entities.Identity;
using Infrastructure.Caching;
using Microsoft.AspNetCore.Identity;
using Persistence.DBConnection;

namespace Infrastructure.Repositories.IdentityRepository
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
