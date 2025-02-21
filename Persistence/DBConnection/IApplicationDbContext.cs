using Domain.Entities;
using Domain.Entities.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Persistence.DBConnection
{
    public interface IApplicationDbContext
    {
        DbSet<Product> Products { get; }
        DbSet<Category> Categories { get; }
        DbSet<Cart> Carts { get; }
        DbSet<Order> Orders { get; }
        DbSet<Payment> Payments { get; }
        DbSet<User> Users { get; }
        //DbSet<UserLogin> UserLogins { get; }
        DbSet<Role> Roles { get; }
        DbSet<Review> Reviews { get; }
        DbSet<CartItem> CartItems { get; }
        DbSet<OrderItem> OrderItems { get; }
        DbSet<FavouriteProduct> FavouriteProducts { get; }
        DbSet<FavouriteItem> FavouriteItems { get; }
        DbSet<Blog> Blogs { get; }
        DbSet<ChatMessage> ChatMessages { get; }
        DbSet<JwtToken> JwtTokens { get; }
        DbSet<Reply> Replies { get; }
        DatabaseFacade Database { get; }
        ChangeTracker ChangeTracker { get; }

        DbSet<TEntity> Set<TEntity>() where TEntity : class;
        ValueTask DisposeAsync();
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
