using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OrderProcessing.Domain.Entities;

namespace OrderProcessing.Infrastructure.Context.Configurations;

public class OutboxMessageConfiguration : IEntityTypeConfiguration<OutboxMessage>
{
    public void Configure(EntityTypeBuilder<OutboxMessage> builder)
    {
        builder.ToTable("OutboxMessages");

        builder.HasKey(m => m.Id);

        builder.Property(i => i.EventType)
            .HasMaxLength(100);

        builder.Property(o => o.Payload)
            .HasColumnType("jsonb");

        builder.Property(o => o.CreatedAt);

        builder.Property(o => o.ProcessedAt)
            .IsRequired(false);
    }
}
