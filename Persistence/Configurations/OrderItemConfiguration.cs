using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations
{
    public class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
    {
        public void Configure(EntityTypeBuilder<OrderItem> builder)
        {
            // Configure primary key
            builder.HasKey(oi => oi.Id);

            builder.Property(x => x.Id)
                  .ValueGeneratedOnAdd()
                  .HasDefaultValueSql("NEWID()");

            // Configure properties
            builder.Property(oi => oi.ProductName)
                .IsRequired()
                .HasMaxLength(100);  // Optimize length based on expected data

            builder.Property(oi => oi.Quantity)
                .IsRequired()
                .HasDefaultValue(1);  // Provide a default value for new records

            builder.Property(oi => oi.UnitPrice)
                .IsRequired()
                .HasColumnType("decimal(18,2)");  // Ensure precision and scale for currency

            // Ignore computed property
            builder.Ignore(oi => oi.TotalPrice);

            // Configure indexes
            builder.HasIndex(oi => oi.OrderId);  // Optimize for queries filtering by OrderId
            builder.HasIndex(oi => oi.ProductId);  // Optimize for queries filtering by ProductId

            // Configure relationships
            builder.HasOne(oi => oi.Order)
                .WithMany(o => o.OrderItems)
                .HasForeignKey(oi => oi.OrderId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(oi => oi.Product)
                .WithMany()
                .HasForeignKey(oi => oi.ProductId)
                .OnDelete(DeleteBehavior.Restrict);  // Prevent deletion of products in use

            builder.ToTable("OrderItems");  // Explicitly name the table
        }
    }
}
