using Contracts.Abstractions;
using Domain.Entities;
using Persistence.Context;

namespace Infrastructure.Repositories
{
    public class CategoryRepository : GenericRepository<Category>, ICategoryRepository
    {
        public CategoryRepository(IApplicationDbContext context) : base(context)
        {
        }
    }
}
