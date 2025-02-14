using Domain.IRepositories.IIdentityRepository;
using Infrastructure.Caching;
using Microsoft.AspNetCore.Identity;
using Persistence.DBConnection;

namespace Infrastructure.Repositories.IdentityRepository
{
    public class RoleRepository : GenericRepository<IdentityRole>, IRoleRepository
    {
        public RoleRepository(IApplicationDbContext context, IRedisCacheService cache) : base(context, cache)
        {

        }
        public RoleManager<IdentityRole> RoleManager { get; }
    }
}
