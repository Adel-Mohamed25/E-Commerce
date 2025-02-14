using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations
{
    public class FavouriteItemConfiguration : IEntityTypeConfiguration<FavouriteItem>
    {
        public void Configure(EntityTypeBuilder<FavouriteItem> builder)
        {
            // Configure primary key
            builder.HasKey(fi => fi.Id);

            builder.Property(x => x.Id)
                  .ValueGeneratedOnAdd()
                  .HasDefaultValueSql("NEWID()");

            // Configure properties
            builder.Property(fi => fi.FavoriteProductId)
                .IsRequired();

            builder.Property(fi => fi.ProductId)
                .IsRequired();

            builder.Property(fi => fi.ProductName)
                .IsRequired()
                .HasMaxLength(100);

            //builder.Property(fi => fi.CreatedDate)
            //    .IsRequired()
            //    .HasDefaultValueSql("GETDATE()");

            // Configure relationships
            builder.HasOne(fi => fi.FavouriteProduct)
                .WithMany(fl => fl.FavoriteItems)
                .HasForeignKey(fi => fi.FavoriteProductId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(fi => fi.Product)
                .WithMany(p => p.FavouriteItems)
                .HasForeignKey(fi => fi.ProductId)
                .OnDelete(DeleteBehavior.Cascade);

            // Indexes and Optimizations
            builder.HasIndex(fi => fi.FavoriteProductId).HasDatabaseName("IX_FavouriteItem_FavoriteListId");
            builder.HasIndex(fi => fi.ProductId).HasDatabaseName("IX_FavouriteItem_ProductId");

            // Table mapping
            builder.ToTable("FavouriteItems");
        }
    }
}
