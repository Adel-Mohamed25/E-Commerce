using Microsoft.AspNetCore.Identity;

namespace Domain.IRepositories.IIdentityRepository
{
    public interface IRoleRepository : IGenericRepository<IdentityRole>
    {
        public RoleManager<IdentityRole> RoleManager { get; }
    }
}
