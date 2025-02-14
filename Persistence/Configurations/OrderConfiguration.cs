using Domain.Entities;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            // Configure primary key
            builder.HasKey(o => o.Id);

            builder.Property(x => x.Id)
                  .ValueGeneratedOnAdd()
                  .HasDefaultValueSql("NEWID()");

            // Configure properties
            builder.Property(o => o.UserId)
                .IsRequired();


            builder.Property(o => o.TotalAmount)
                .IsRequired()
                .HasColumnType("decimal(18,2)");

            builder.Property(o => o.Status)
                .IsRequired()
                .HasConversion(
                    v => v.ToString(),
                    v => Enum.Parse<OrderStatus>(v))
                .HasMaxLength(20);

            builder.Property(o => o.ShippingAddress)
                .HasMaxLength(250);

            builder.Property(o => o.BillingAddress)
                .HasMaxLength(250);

            builder.Property(o => o.TrackingNumber)
                .HasMaxLength(50);

            // Configure indexes
            builder.HasIndex(o => o.CreatedDate);  // Optimize for queries based on OrderDate
            builder.HasIndex(o => o.Status);     // Optimize for queries based on Status
            builder.HasIndex(o => o.TrackingNumber).IsUnique(); // Ensure unique tracking numbers

            // Configure relationships
            builder.HasOne(o => o.User)
                .WithMany(u => u.OrderList)  // Assuming User has a collection of Orders
                .HasForeignKey(o => o.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(o => o.OrderItems)
                .WithOne(oi => oi.Order)
                .HasForeignKey(oi => oi.OrderId)
                .OnDelete(DeleteBehavior.Cascade);

            // Optional: Configure the entity as a table with optimized settings
            builder.ToTable("Orders");  // Explicitly name the table
        }
    }
}
