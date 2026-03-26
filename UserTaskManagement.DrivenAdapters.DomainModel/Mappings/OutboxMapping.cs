using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UserTaskManagement.Application.IntegrationEvents;

namespace UserTaskManagement.DrivenAdapters.DomainModel.Mappings;

/// <summary>
/// Конфигурация таблицы аутбокса
/// </summary>
internal class OutboxMessageConfiguration : IEntityTypeConfiguration<OutboxMessage>
{
    public void Configure(EntityTypeBuilder<OutboxMessage> builder)
    {
        builder.ToTable("outbox_message");
        
        builder.HasKey(x => x.Id);
        
        builder.Property(x => x.Type)
            .IsRequired()
            .HasMaxLength(255)
            .HasColumnName("type");
        
        builder.Property(x => x.Data)
            .IsRequired()
            .HasColumnType("jsonb")
            .HasColumnName("data");
        
        builder.Property(x => x.OccurredOn)
            .IsRequired()
            .HasColumnType("timestamp with time zone")
            .HasColumnName("occurred_on");
        
        builder.Property(x => x.ProcessedAt)
            .HasColumnType("timestamp with time zone")
            .HasColumnName("processed_at");
        
        builder.Property(x => x.RetryCount)
            .HasDefaultValue(0)
            .HasColumnName("retry_count");
        
        builder
            .HasIndex(x => new { x.ProcessedAt, x.OccurredOn })
            .HasDatabaseName("ix_outbox_message_unprocessed")
            .HasFilter("processed_at IS NULL");
        
        builder
            .HasIndex(x => x.Type)
            .HasDatabaseName("ix_outbox_message_type");
    }
}
