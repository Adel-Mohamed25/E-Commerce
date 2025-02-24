using Domain.Entities.Identity;

namespace Contracts.Abstractions.IdentityRepositories
{
    public interface IJwtTokenRepository : IGenericRepository<JwtToken>
    {
    }
}
