using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace UserTaskManagement.DrivenAdapters.DomainModel;

/// <summary>
/// для миграций
/// </summary>
public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
{
    public AppDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
        
        // Строка подключения по умолчанию
        var connectionString = "Host=localhost;Database=UserTaskManagementDb;Username=postgres;Password=yourpassword";
        
        // Если переданы аргументы, используем первый как строку подключения
        if (args != null! && args.Length > 0 && !string.IsNullOrEmpty(args[0]))
        {
            connectionString = args[0];
        }
        
        optionsBuilder.UseNpgsql(connectionString);
        
        return new AppDbContext(optionsBuilder.Options);
    }
}
