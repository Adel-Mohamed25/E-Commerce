using Contracts.Repositories.IdentityRepositories;
using Domain.Entities.Identity;
using Infrastructure.Caching;
using Persistence.Context;

namespace Infrastructure.Repositories.IdentityRepositories
{
    public class UserLoginRepository : GenericRepository<UserLogin>, IUserLoginRepository
    {
        public UserLoginRepository(IApplicationDbContext context,
            IRedisCacheService cache) : base(context, cache) { }

    }
}
