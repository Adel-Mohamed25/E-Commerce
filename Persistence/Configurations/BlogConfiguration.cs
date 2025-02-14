using Domain.Entities;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations
{
    public class BlogConfiguration : IEntityTypeConfiguration<Blog>
    {
        public void Configure(EntityTypeBuilder<Blog> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                  .ValueGeneratedOnAdd()
                  .HasDefaultValueSql("NEWID()");

            builder.Property(b => b.Type)
                .HasConversion(b => b.ToString(), b => Enum.Parse<BlogType>(b));

            builder.Property(b => b.Title).
                IsRequired()
                .HasMaxLength(50);


            builder.Property(b => b.Description).
                IsRequired()
                .HasMaxLength(500);

            builder.Property(b => b.Author).
                IsRequired()
                .HasMaxLength(500);

            builder.HasIndex(b => b.Title)
                .IsClustered(false);

            builder.ToTable("Blogs");
        }
    }
}
