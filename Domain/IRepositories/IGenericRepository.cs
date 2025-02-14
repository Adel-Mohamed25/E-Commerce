﻿using Domain.Enums;
using System.Linq.Expressions;

namespace Domain.IRepositories
{
    public interface IGenericRepository<TEntity> where TEntity : class
    {
        #region Commands
        Task CreateAsync(TEntity entity, CancellationToken cancellationToken = default);
        Task CreateRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default);
        Task UpdateAsync(TEntity entity, CancellationToken cancellationToken = default);
        Task UpdatedRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default);
        Task ExecuteUpdateAsync(Func<TEntity, object> propertySelector,
                                Expression<Func<TEntity, object>> valueSelector,
                                Expression<Func<TEntity, bool>>? filter = null,
                                CancellationToken cancellationToken = default);
        Task DeleteAsync(TEntity entity, CancellationToken cancellationToken = default);
        Task ExecuteDeleteAsync(Expression<Func<TEntity, bool>>? filter = null,
                                CancellationToken cancellationToken = default);

        #endregion

        #region Queries
        Task<TEntity> GetByAsync(Expression<Func<TEntity, bool>> mandatoryFilter,
                                    Expression<Func<TEntity, bool>>? optionalFilter = null,
                                    CancellationToken cancellationToken = default,
                                    params string[] includes);

        Task<IQueryable<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>>? firstFilter = null,
                                                   Expression<Func<TEntity, bool>>? secondFilter = null,
                                                   Expression<Func<TEntity, object>>? orderBy = null,
                                                   OrderByDirection orderByDirection = OrderByDirection.Ascending,
                                                   int? pageNumber = null,
                                                   int? pageSize = null,
                                                   bool paginationOn = false,
                                                   CancellationToken cancellationToken = default,
                                                   params string[] includes);

        Task<bool> IsExistAsync(Expression<Func<TEntity, bool>>? filter = null,
                                CancellationToken cancellationToken = default);

        Task<int> CountAsync(Expression<Func<TEntity, bool>>? filter = null,
                               CancellationToken cancellationToken = default);
        #endregion

    }
}
