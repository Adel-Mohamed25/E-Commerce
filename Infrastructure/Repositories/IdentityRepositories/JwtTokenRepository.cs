using Contracts.Abstractions.IdentityRepositories;
using Domain.Entities.Identity;
using Persistence.Context;

namespace Infrastructure.Repositories.IdentityRepositories
{
    public class JwtTokenRepository : GenericRepository<JwtToken>, IJwtTokenRepository
    {
        public JwtTokenRepository(IApplicationDbContext context) : base(context)
        {

        }
    }
}
