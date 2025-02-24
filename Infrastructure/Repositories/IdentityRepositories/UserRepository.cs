using Contracts.Abstractions.IdentityRepositories;
using Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Persistence.Context;

namespace Infrastructure.Repositories.IdentityRepositories
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        public UserRepository(UserManager<User> userManager,
            SignInManager<User> signInManager,
            IApplicationDbContext context) : base(context)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public UserManager<User> UserManager { get; }
        public SignInManager<User> SignInManager { get; }

    }
}
