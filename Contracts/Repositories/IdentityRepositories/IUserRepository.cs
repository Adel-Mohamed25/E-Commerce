using Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity;

namespace Contracts.Repositories.IdentityRepositories
{
    public interface IUserRepository : IGenericRepository<User>
    {
        SignInManager<User> SignInManager { get; }

        UserManager<User> UserManager { get; }
    }
}
