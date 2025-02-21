using Domain.Entities.Identity;

namespace Contracts.Contracts.IIdentityRepository
{
    public interface IJwtTokenRepository : IGenericRepository<JwtToken>
    {
    }
}
