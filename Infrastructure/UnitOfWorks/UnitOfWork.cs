﻿using Domain.Entities;
using Domain.IRepositories;
using Domain.IRepositories.IIdentityRepository;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;
using Persistence.DBConnection;

namespace Infrastructure.UnitOfWorks
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IApplicationDbContext _context;
        private readonly ILogger<UnitOfWork> _logger;

        public ICategoryRepository Categories { get; private set; }
        public IUserRepository Users { get; private set; }

        public UnitOfWork(IApplicationDbContext context,
            ILogger<UnitOfWork> logger,
            ICategoryRepository categories,
            IUserRepository users)
        {
            _context = context;
            _logger = logger;
            Categories = categories;
            Users = users;
        }

        public async ValueTask DisposeAsync()
        {
            await _context.DisposeAsync();
        }

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken)
        {
            try
            {
                foreach (var entry in _context.ChangeTracker.Entries<BaseEntity>())
                {
                    switch (entry.State)
                    {
                        case EntityState.Added:
                            entry.Entity.CreatedDate = DateTime.UtcNow;
                            break;
                        case EntityState.Modified:
                            entry.Entity.ModifiedDate = DateTime.UtcNow;
                            break;
                    }
                }

                return await _context.SaveChangesAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while saving changes in UnitOfWork.");
                throw;
            }
        }

        public async Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default)
        {
            return await _context.Database.BeginTransactionAsync(cancellationToken);
        }

        public async Task CommitTransactionAsync(CancellationToken cancellationToken = default)
        {
            await _context.Database.CommitTransactionAsync(cancellationToken);
        }

        public async Task RollbackTransactionAsync(CancellationToken cancellationToken = default)
        {
            await _context.Database.RollbackTransactionAsync(cancellationToken);
        }

    }
}
