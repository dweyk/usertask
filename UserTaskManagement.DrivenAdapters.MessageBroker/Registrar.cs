using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using UserTaskManagement.Application.DrivenPorts.MessageBrokerPort;

namespace UserTaskManagement.DrivenAdapters.MessageBroker;

public static class Registrar
{
    /// <summary>
    /// Регистрация брокера сообщений
    /// </summary>
    public static IServiceCollection RegisterMessageBroker(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        services.Configure<KafkaOptions>(configuration.GetSection("Kafka"));
        services.AddSingleton<IMessageBrokerPort, KafkaMessageBrokerAdapter>();
        
        return services;
    }
}
