using Domain.Entities;
using Domain.IRepositories;
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
