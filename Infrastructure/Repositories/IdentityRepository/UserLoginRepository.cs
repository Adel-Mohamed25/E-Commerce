using Contracts.Contracts.IIdentityRepository;
using Domain.Entities.Identity;
using Infrastructure.Caching;
using Persistence.DBConnection;

namespace Infrastructure.Repositories.IdentityRepository
{
    public class UserLoginRepository : GenericRepository<UserLogin>, IUserLoginRepository
    {
        public UserLoginRepository(IApplicationDbContext context,
            IRedisCacheService cache) : base(context, cache) { }

    }
}
