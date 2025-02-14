using Domain.Entities;
using Domain.Entities.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations
{
    public class FavouriteProductConfiguration : IEntityTypeConfiguration<FavouriteProduct>
    {
        public void Configure(EntityTypeBuilder<FavouriteProduct> builder)
        {
            // Configure primary key
            builder.HasKey(fl => fl.Id);

            builder.Property(x => x.Id)
                  .ValueGeneratedOnAdd()
                  .HasDefaultValueSql("NEWID()");

            // Configure properties
            builder.Property(fl => fl.UserId)
                .IsRequired();

            //builder.Property(fl => fl.CreatedDate)
            //    .IsRequired()
            //    .HasDefaultValueSql("GETDATE()");

            //builder.Property(fl => fl.ModifiedDate)
            //    .IsRequired(false);

            // Configure relationships
            builder.HasOne(fl => fl.User)
                .WithOne(u => u.FavouriteProduct)
                .HasForeignKey<User>(u => u.FavouriteProductId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(fl => fl.FavoriteItems)
                .WithOne(fi => fi.FavouriteProduct)
                .HasForeignKey(fi => fi.FavoriteProductId)
                .OnDelete(DeleteBehavior.Cascade);

            // Indexes and Optimizations
            builder.HasIndex(fl => fl.UserId)
                .HasDatabaseName("IX_FavouriteList_UserId");

            // Table mapping
            builder.ToTable("FavouriteProducts");
        }
    }
}
