using Contracts.Repositories;
using Domain.Entities;
using Infrastructure.Caching;
using Persistence.Context;

namespace Infrastructure.Repositories
{
    public class CategoryRepository : GenericRepository<Category>, ICategoryRepository
    {
        public CategoryRepository(IApplicationDbContext context, IRedisCacheService cache) : base(context, cache)
        {
        }
    }
}
