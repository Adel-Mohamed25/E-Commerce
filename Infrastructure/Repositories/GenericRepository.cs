using Contracts.Contracts;
using Domain.Enums;
using Infrastructure.Caching;
using Microsoft.EntityFrameworkCore;
using Persistence.DBConnection;
using System.Linq.Expressions;

namespace Infrastructure.Repositories
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
    {
        private readonly IApplicationDbContext _context;
        private readonly IRedisCacheService _cache;
        private readonly DbSet<TEntity> _dbset;
        public GenericRepository(IApplicationDbContext context, IRedisCacheService cache)
        {
            _context = context;
            _cache = cache;
            _dbset = _context.Set<TEntity>();
        }


        public virtual async Task CreateAsync(TEntity entity, CancellationToken cancellationToken = default)
        {
            await _dbset.AddAsync(entity, cancellationToken);
            _cache.RemoveData("Entities");
        }

        public virtual async Task CreateRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
        {
            await _dbset.AddRangeAsync(entities, cancellationToken);
            _cache.RemoveData("Entities");
        }


        public virtual async Task UpdateAsync(TEntity entity, CancellationToken cancellationToken = default)
        {
            _dbset.Update(entity);
            _cache.RemoveData("Entities");
            await Task.CompletedTask;
        }

        public virtual async Task UpdatedRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
        {
            if (entities is not null)
            {
                _dbset.UpdateRange(entities);
                _cache.RemoveData("Entities");
            }
            await Task.CompletedTask;
        }


        public virtual async Task ExecuteUpdateAsync(Func<TEntity, object> propertySelector, Expression<Func<TEntity, object>> valueSelector, Expression<Func<TEntity, bool>>? filter = null, CancellationToken cancellationToken = default)
        {
            if (filter is null)
                await _dbset.ExecuteUpdateAsync(entity => entity.SetProperty(propertySelector, valueSelector), cancellationToken);

            else
                await _dbset.Where(filter).ExecuteUpdateAsync(entity => entity.SetProperty(propertySelector, valueSelector), cancellationToken);

            _cache.RemoveData("Entities");
        }

        public virtual async Task DeleteAsync(TEntity entity, CancellationToken cancellationToken = default)
        {
            await Task.FromResult(_dbset.Remove(entity));
            _cache.RemoveData("Entities");
        }

        public virtual async Task ExecuteDeleteAsync(Expression<Func<TEntity, bool>>? filter = null, CancellationToken cancellationToken = default)
        {
            if (filter is null)
                await _dbset.ExecuteDeleteAsync(cancellationToken);
            else
                await _dbset.Where(filter).ExecuteDeleteAsync(cancellationToken);
            _cache.RemoveData("Entities");
        }

        public virtual async Task<TEntity>
            GetByAsync(Expression<Func<TEntity, bool>> mandatoryFilter,
                            Expression<Func<TEntity, bool>>? optionalFilter = null,
                            CancellationToken cancellationToken = default,
                            params string[] includes)
        {
            IQueryable<TEntity> entities = _dbset.AsNoTracking().AsQueryable();

            if (optionalFilter is null)
                entities = entities.Where(mandatoryFilter);
            else
                entities = entities.Where(mandatoryFilter).Where(optionalFilter);

            if (includes?.Length > 0)
            {
                foreach (var include in includes)
                {
                    entities = entities.Include(include);
                }
            }

            return await entities.FirstOrDefaultAsync(cancellationToken);
        }

        public virtual async Task<IQueryable<TEntity>>
            GetAllAsync(Expression<Func<TEntity, bool>>? firstFilter = null,
                             Expression<Func<TEntity, bool>>? secondFilter = null,
                             Expression<Func<TEntity, object>>? orderBy = null,
                             OrderByDirection orderByDirection = OrderByDirection.Ascending,
                             int? pageNumber = null,
                             int? pageSize = null,
                             bool ispagination = false,
                             CancellationToken cancellationToken = default,
                             params string[] includes)
        {
            IQueryable<TEntity> entities = _dbset.AsNoTracking().AsQueryable();

            if (firstFilter is not null)
                entities = entities.Where(firstFilter);

            if (secondFilter is not null)
                entities = entities.Where(secondFilter);

            if (orderBy is not null)
            {
                if (orderByDirection == OrderByDirection.Ascending)
                    entities = entities.OrderBy(orderBy);
                else
                    entities = entities.OrderByDescending(orderBy);
            }

            if (ispagination)
            {
                pageNumber = pageNumber.HasValue ? pageNumber.Value <= 0 ? 1 : pageNumber.Value : 1;
                pageSize = pageSize.HasValue ? pageSize.Value <= 0 ? 5 : pageSize.Value : 5;
                entities = entities.Skip((pageNumber.Value - 1) * pageSize.Value).Take(pageSize.Value);
            }

            if (includes?.Length > 0)
            {
                foreach (var include in includes)
                {
                    entities = entities.Include(include);
                }
            }

            var data = _cache.GetData<List<TEntity>>("Entities");
            if (data != null && data.Any())
            {
                return data.AsQueryable();
            }

            var list = await entities.ToListAsync(cancellationToken);
            if (list.Any())
            {
                _cache.SetData("Entities", list);
            }

            return list.AsQueryable();
        }



        public virtual async Task<bool> IsExistAsync(Expression<Func<TEntity, bool>>? filter = null, CancellationToken cancellationToken = default)
        {
            if (filter is null)
                return await _dbset.AnyAsync(cancellationToken);
            return await _dbset.AnyAsync(filter, cancellationToken);
        }

        public virtual async Task<int> CountAsync(Expression<Func<TEntity, bool>>? filter = null, CancellationToken cancellationToken = default)
        {
            if (filter is null)
                return await _dbset.CountAsync(cancellationToken);
            return await _dbset.CountAsync(filter, cancellationToken);
        }
    }
}
