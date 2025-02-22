using Contracts.Repositories.IdentityRepositories;
using Domain.Entities.Identity;
using Infrastructure.Caching;
using Persistence.Context;

namespace Infrastructure.Repositories.IdentityRepositories
{
    public class JwtTokenRepository : GenericRepository<JwtToken>, IJwtTokenRepository
    {
        public JwtTokenRepository(IApplicationDbContext context, IRedisCacheService cache) : base(context, cache)
        {

        }
    }
}
