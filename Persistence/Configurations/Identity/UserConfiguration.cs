using Domain.Entities.Identity;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations.Identity
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(u => u.Id);

            builder.Property(u => u.Id)
                   .ValueGeneratedOnAdd()
                   .HasDefaultValueSql("NEWID()");

            // Configure properties
            builder.Property(u => u.FirstName)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(u => u.LastName)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(u => u.Address)
                .HasMaxLength(200)
                .IsRequired(false);

            builder.Property(u => u.Age)
                .IsRequired(false);

            builder.Property(u => u.Image)
                .HasMaxLength(200)
                .IsRequired(false);

            builder.Property(u => u.DateOfBirth)
                .IsRequired(false);

            builder.Property(u => u.Gender)
                .HasMaxLength(6)
                .IsRequired()
                .HasConversion(u => u.ToString(),
                u => Enum.Parse<GenderType>(u));


            //builder.Property(u => u.CreatedDate)
            //    .IsRequired()
            //    .HasDefaultValueSql("GETDATE()");

            builder.Property(u => u.LastLoginDate)
                .IsRequired(false);



            // Configure relationships
            builder.HasOne(u => u.Cart)
                .WithOne(c => c.User)
                .HasForeignKey<User>(u => u.CartId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(u => u.FavouriteProduct)
                .WithOne(fl => fl.User)
                .HasForeignKey<User>(u => u.FavouriteProductId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(u => u.OrderList)
                .WithOne(o => o.User)
                .HasForeignKey(o => o.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // Here i configure it as No Action to prevent Circular Cascade

            builder.HasMany(u => u.PaymentList)
                .WithOne(p => p.User)
                .HasForeignKey(p => p.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasMany(u => u.ReviewsList)
                .WithOne(r => r.User)
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // Indexes and Optimizations
            builder.HasIndex(u => u.Email)
                .IsUnique()
                .HasDatabaseName("IX_User_Email");

            // Table mapping
            builder.ToTable("Users");
        }
    }
}
