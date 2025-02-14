using Domain.Entities;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations
{
    public class PaymentConfiguration : IEntityTypeConfiguration<Payment>
    {
        public void Configure(EntityTypeBuilder<Payment> builder)
        {
            // Configure primary key
            builder.HasKey(p => p.Id);

            builder.Property(x => x.Id)
                  .ValueGeneratedOnAdd()
                  .HasDefaultValueSql("NEWID()");

            // Configure properties
            builder.Property(p => p.Amount)
                .IsRequired()
                .HasColumnType("decimal(18,2)");

            builder.Property(p => p.Method)
                .IsRequired()
                .HasConversion(
                    v => v.ToString(),
                    v => Enum.Parse<PaymentMethod>(v))
                .HasMaxLength(20)
                .HasDefaultValue(PaymentMethod.CreditCard);

            builder.Property(p => p.Status)
                .IsRequired()
                .HasConversion(
                    v => v.ToString(),
                    v => Enum.Parse<PaymentStatus>(v))
                .HasMaxLength(20)
                .HasDefaultValue(PaymentStatus.Pending);


            builder.Property(p => p.ConfirmationDate)
                .IsRequired(false);

            builder.Property(p => p.TransactionId)
                .HasMaxLength(100);

            // Configure relationships


            builder.HasOne(p => p.User)
                .WithMany(u => u.PaymentList)
                .HasForeignKey(p => p.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            // Indexes for optimization
            builder.HasIndex(p => p.OrderId).HasDatabaseName("IX_Payments_OrderId");
            builder.HasIndex(p => p.UserId).HasDatabaseName("IX_Payments_UserId");

            // Map to table
            builder.ToTable("Payments");
        }
    }
}
