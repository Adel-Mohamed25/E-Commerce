using Domain.Entities.Identity;
using Domain.IRepositories.IIdentityRepository;
using Infrastructure.Caching;
using Microsoft.AspNetCore.Identity;
using Persistence.DBConnection;

namespace Infrastructure.Repositories.IdentityRepository
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        public UserRepository(IApplicationDbContext context, IRedisCacheService cache) : base(context, cache)
        {
        }

        public SignInManager<User> SignInManager { get; }

        public UserManager<User> UserManager { get; }
    }
}
