using Contracts.Contracts;
using Domain.Entities;
using Infrastructure.Caching;
using Persistence.DBConnection;

namespace Infrastructure.Repositories
{
    public class CategoryRepository : GenericRepository<Category>, ICategoryRepository
    {
        public CategoryRepository(IApplicationDbContext context, IRedisCacheService cache) : base(context, cache)
        {
        }
    }
}
