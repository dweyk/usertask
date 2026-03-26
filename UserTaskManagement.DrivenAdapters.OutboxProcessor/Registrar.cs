using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace UserTaskManagement.DrivenAdapters.OutboxProcessor;

public static class Registrar
{
    /// <summary>
    /// Регистрация аутбокса
    /// </summary>
    public static IServiceCollection RegisterOutboxProcessor(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        services.Configure<OutboxProcessorOptions>(configuration.GetSection("OutboxProcessor"));
        services.AddHostedService<OutboxProcessorService>();
        
        return services;
    }
}
