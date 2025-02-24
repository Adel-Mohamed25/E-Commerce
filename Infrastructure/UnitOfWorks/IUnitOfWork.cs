using Contracts.Abstractions;
using Contracts.Abstractions.IdentityRepositories;
using Microsoft.EntityFrameworkCore.Storage;

namespace Infrastructure.UnitOfWorks
{
    public interface IUnitOfWork : IAsyncDisposable
    {
        ICategoryRepository Categories { get; }
        IUserRepository Users { get; }
        IRoleRepository Roles { get; }
        IJwtTokenRepository JwtTokens { get; }
        IUserLoginRepository UserLogins { get; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
        Task CommitTransactionAsync(CancellationToken cancellationToken = default);
        Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default);
        Task RollbackTransactionAsync(CancellationToken cancellationToken = default);
    }
}
