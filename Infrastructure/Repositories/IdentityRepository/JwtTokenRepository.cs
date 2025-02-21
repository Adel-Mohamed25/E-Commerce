using Contracts.Contracts.IIdentityRepository;
using Domain.Entities.Identity;
using Infrastructure.Caching;
using Persistence.DBConnection;

namespace Infrastructure.Repositories.IdentityRepository
{
    public class JwtTokenRepository : GenericRepository<JwtToken>, IJwtTokenRepository
    {
        public JwtTokenRepository(IApplicationDbContext context, IRedisCacheService cache) : base(context, cache)
        {

        }
    }
}
