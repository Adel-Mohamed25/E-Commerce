using Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity;

namespace Domain.IRepositories.IIdentityRepository
{
    public interface IUserRepository : IGenericRepository<User>
    {
        SignInManager<User> SignInManager { get; }

        UserManager<User> UserManager { get; }
    }
}
