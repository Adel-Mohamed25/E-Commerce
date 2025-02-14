using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations
{
    public class ReviewConfiguration : IEntityTypeConfiguration<Review>
    {
        public void Configure(EntityTypeBuilder<Review> builder)
        {
            // Configure primary key
            builder.HasKey(r => r.Id);

            builder.Property(x => x.Id)
                  .ValueGeneratedOnAdd()
                  .HasDefaultValueSql("NEWID()");

            // Configure properties
            builder.Property(r => r.Rating)
                .IsRequired()
                .HasColumnType("int");

            builder.Property(r => r.ReviewText)
                .IsRequired()
                .HasMaxLength(1000);

            builder.Property(r => r.UserId)
                .IsRequired();

            builder.Property(r => r.ProductId)
                .IsRequired();

            // Configure relationships
            builder.HasOne(r => r.User)
                .WithMany(u => u.ReviewsList) // Assuming a User can have many Reviews; if not, adjust accordingly
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(r => r.Product)
                .WithMany(p => p.ReviewsList) // Assuming a Product can have many Reviews
                .HasForeignKey(r => r.ProductId)
                .OnDelete(DeleteBehavior.Cascade);

            // Indexes and Optimizations
            builder.HasIndex(r => r.UserId)
                .HasDatabaseName("IX_Review_UserId");

            builder.HasIndex(r => r.ProductId)
                .HasDatabaseName("IX_Review_ProductId");

            // Table mapping
            builder.ToTable("Reviews");
        }
    }
}
