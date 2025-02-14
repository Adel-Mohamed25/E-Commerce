using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations
{
    public class ChatMessageConfiguration : IEntityTypeConfiguration<ChatMessage>
    {
        public void Configure(EntityTypeBuilder<ChatMessage> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                  .ValueGeneratedOnAdd()
                  .HasDefaultValueSql("NEWID()");

            builder.Property(c => c.Message)
                .IsRequired();

            builder.Property(b => b.ReceiverId)
                .IsRequired();

            builder.Property(b => b.SenderId)
                .IsRequired();

            //builder.Property(b => b.CreatedDate)
            //    .IsRequired()
            //    .HasDefaultValue(DateTime.UtcNow);

            builder.ToTable("ChatMessages");
        }
    }
}
