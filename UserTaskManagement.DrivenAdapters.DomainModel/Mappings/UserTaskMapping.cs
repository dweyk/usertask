using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UserTaskManagement.Domain.Models.UserTask;

namespace UserTaskManagement.DrivenAdapters.DomainModel.Mappings;

/// <summary>
/// Конфигурация таблицы пользовательских задач
/// </summary>
internal sealed class UserTaskConfiguration : IEntityTypeConfiguration<UserTask>
{
    public void Configure(EntityTypeBuilder<UserTask> builder)
    {
        builder.ToTable("user_tasks");
        
        builder.HasKey(x => x.Id);
        
        // Оптимистическая блокировка
        builder.Property(x => x.Version)
            .IsRowVersion()
            .HasColumnName("xmin")
            .HasColumnType("xid")
            .ValueGeneratedOnAddOrUpdate();
        
        builder.Property(x => x.Title)
            .IsRequired()
            .HasMaxLength(200)
            .HasColumnName("title");
            
        builder.Property(x => x.Description)
            .IsRequired()
            .HasMaxLength(2000)
            .HasColumnName("description");
        
        builder.Property(x => x.Status)
            .HasConversion<string>()
            .HasMaxLength(50)
            .HasColumnName("status");
            
        builder.Property(x => x.UserId)
            .IsRequired()
            .HasColumnName("user_id");
            
        builder.Property(x => x.CreatedAt)
            .IsRequired()
            .HasColumnName("created_at");
            
        builder.Property(x => x.UpdatedAt)
            .IsRequired()
            .HasColumnName("updated_at");
        
        builder.HasIndex(x => x.UserId)
            .HasDatabaseName("ix_user_tasks_user_id");
            
        builder.HasIndex(x => x.Status)
            .HasDatabaseName("ix_user_tasks_status");
            
        builder.HasIndex(x => x.CreatedAt)
            .HasDatabaseName("ix_user_tasks_created_at");
            
        builder.HasIndex(x => x.UpdatedAt)
            .HasDatabaseName("ix_user_tasks_updated_at");
            
        builder.HasIndex(x => x.Title)
            .HasDatabaseName("ix_user_tasks_title");
    }
}