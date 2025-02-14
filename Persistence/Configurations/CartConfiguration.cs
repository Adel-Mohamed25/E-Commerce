using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations
{
    public class CartConfiguration : IEntityTypeConfiguration<Cart>
    {
        public void Configure(EntityTypeBuilder<Cart> builder)
        {
            builder.HasKey(c => c.Id);

            builder.Property(x => x.Id)
                  .ValueGeneratedOnAdd()
                  .HasDefaultValueSql("NEWID()");

            // Configure Properties 
            builder.Property(c => c.UserId)
                .IsRequired();

            builder.Property(c => c.CreatedDate)
                .IsRequired();

            //builder.Property(c => c.ModifiedDate)
            //.HasColumnName("ModifiedDate")
            //.HasDefaultValueSql("GETDATE()") // Default value if not provided
            //.ValueGeneratedOnAddOrUpdate() // Indicates that it’s managed by the database
            //.Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore); // Ignore property during save operations

            // Configure relationships
            builder.HasOne(c => c.User)
                .WithOne(c => c.Cart)
                .HasForeignKey<Cart>(c => c.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(c => c.CartItems)
                .WithOne(ci => ci.Cart)
                .HasForeignKey(ci => ci.CartId)
                .OnDelete(DeleteBehavior.Cascade);


            // Assuming a scalar function GetTotalAmount exists in the database
            // Optional: Configure TotalAmount as computed column
            //builder.Property(c => c.TotalAmount)
            // .HasComputedColumnSql("[dbo].[GetTotalAmount](Id)");  

            builder.ToTable("Carts");
        }
    }
}
