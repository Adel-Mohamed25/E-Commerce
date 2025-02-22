using Contracts.Repositories.IdentityRepositories;
using Domain.Entities.Identity;
using Infrastructure.Caching;
using Microsoft.AspNetCore.Identity;
using Persistence.Context;

namespace Infrastructure.Repositories.IdentityRepositories
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        public UserRepository(UserManager<User> userManager,
            SignInManager<User> signInManager,
            IApplicationDbContext context,
            IRedisCacheService cache) : base(context, cache)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public UserManager<User> UserManager { get; }
        public SignInManager<User> SignInManager { get; }

    }
}
