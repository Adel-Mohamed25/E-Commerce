using Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity;

namespace Contracts.Repositories.IdentityRepositories
{
    public interface IRoleRepository : IGenericRepository<Role>
    {
        public RoleManager<Role> RoleManager { get; }
    }
}
