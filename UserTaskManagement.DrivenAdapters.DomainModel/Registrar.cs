using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using UserTaskManagement.Application;
using UserTaskManagement.Application.DrivenPorts.DomainModel;
using UserTaskManagement.DrivenAdapters.DomainModel.Repositories;

namespace UserTaskManagement.DrivenAdapters.DomainModel;

public static class Registrar
{
    /// <summary>
    /// Зарегестировать доступ к хранилищу домена
    /// </summary>
    public static IServiceCollection RegisterDomainPersistence(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");

        services.AddDbContext<AppDbContext>(
            options => options.UseNpgsql(
                connectionString,
                npgsql => npgsql.MigrationsAssembly(typeof(AppDbContext).Assembly.FullName)
            )
        );

        services.AddScoped<IUserTaskRepository, UserTaskRepository>();
        services.AddScoped<IOutboxRepository, OutboxRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        return services;
    }
}
