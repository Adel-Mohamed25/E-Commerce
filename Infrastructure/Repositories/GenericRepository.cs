﻿using Contracts.Abstractions;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Persistence.Context;
using System.Linq.Expressions;

namespace Infrastructure.Repositories
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
    {
        private readonly IApplicationDbContext _context;
        private readonly DbSet<TEntity> _dbset;
        public GenericRepository(IApplicationDbContext context)
        {
            _context = context;
            _dbset = _context.Set<TEntity>();
        }


        public virtual async Task CreateAsync(TEntity entity, CancellationToken cancellationToken = default)
        {
            await _dbset.AddAsync(entity, cancellationToken);
        }

        public virtual async Task CreateRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
        {
            await _dbset.AddRangeAsync(entities, cancellationToken);
        }


        public virtual async Task UpdateAsync(TEntity entity, CancellationToken cancellationToken = default)
        {
            _dbset.Update(entity);
            await Task.CompletedTask;
        }

        public virtual async Task UpdatedRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
        {
            if (entities is not null)
            {
                _dbset.UpdateRange(entities);
            }
            await Task.CompletedTask;
        }


        public virtual async Task ExecuteUpdateAsync(Func<TEntity, object> propertySelector, Expression<Func<TEntity, object>> valueSelector, Expression<Func<TEntity, bool>>? filter = null, CancellationToken cancellationToken = default)
        {
            if (filter is null)
                await _dbset.ExecuteUpdateAsync(entity => entity.SetProperty(propertySelector, valueSelector), cancellationToken);

            else
                await _dbset.Where(filter).ExecuteUpdateAsync(entity => entity.SetProperty(propertySelector, valueSelector), cancellationToken);

        }

        public virtual async Task DeleteAsync(TEntity entity, CancellationToken cancellationToken = default)
        {
            await Task.FromResult(_dbset.Remove(entity));
        }

        public virtual async Task ExecuteDeleteAsync(Expression<Func<TEntity, bool>>? filter = null, CancellationToken cancellationToken = default)
        {
            if (filter is null)
                await _dbset.ExecuteDeleteAsync(cancellationToken);
            else
                await _dbset.Where(filter).ExecuteDeleteAsync(cancellationToken);
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
                pageSize = pageSize.HasValue ? pageSize.Value <= 0 ? 10 : pageSize.Value : 10;
                entities = entities.Skip((pageNumber.Value - 1) * pageSize.Value).Take(pageSize.Value);
            }

            if (includes?.Length > 0)
            {
                foreach (var include in includes)
                {
                    entities = entities.Include(include);
                }
            }


            return await Task.FromResult(entities);
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
