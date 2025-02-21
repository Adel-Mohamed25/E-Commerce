using Contracts.Contracts.IIdentityRepository;
using Domain.Entities.Identity;
using Infrastructure.Caching;
using Microsoft.AspNetCore.Identity;
using Persistence.DBConnection;

namespace Infrastructure.Repositories.IdentityRepository
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
