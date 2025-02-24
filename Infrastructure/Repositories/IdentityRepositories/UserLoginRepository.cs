using Contracts.Abstractions.IdentityRepositories;
using Domain.Entities.Identity;
using Persistence.Context;

namespace Infrastructure.Repositories.IdentityRepositories
{
    public class UserLoginRepository : GenericRepository<UserLogin>, IUserLoginRepository
    {
        public UserLoginRepository(IApplicationDbContext context) : base(context) { }

    }
}
