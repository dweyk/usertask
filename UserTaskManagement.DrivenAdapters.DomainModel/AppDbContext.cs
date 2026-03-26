using Microsoft.EntityFrameworkCore;
using UserTaskManagement.Application.IntegrationEvents;
using UserTaskManagement.Domain.Models.UserTask;
using UserTaskManagement.DrivenAdapters.DomainModel.Mappings;

namespace UserTaskManagement.DrivenAdapters.DomainModel;

public class AppDbContext : DbContext
{
    public DbSet<UserTask> UserTasks { get; set; } = null!;
    
    public DbSet<OutboxMessage> OutboxMessages { get; set; } = null!;

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new UserTaskConfiguration());
        modelBuilder.ApplyConfiguration(new OutboxMessageConfiguration());
    }
}
