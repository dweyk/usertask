using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using UserTaskManagement.Application.UseCases;
using UserTaskManagement.Client.UserData;
using UserTaskManagement.Domain.Factories;
using UserTaskManagement.DrivenAdapters.DomainModel;
using UserTaskManagement.DrivenAdapters.MessageBroker;
using UserTaskManagement.DrivenAdapters.OutboxProcessor;
using UserTaskManagement.DrivenAdapters.UserData;

namespace UserTaskManagement;

internal static class Registrar
{
    /// <summary>
    /// Регистрация зависимостей сервиса
    /// </summary>
    /// <param name="services">Дескриптор сервисов</param>
    /// <param name="configuration">Конфигурация</param>
    internal static IServiceCollection RegisterServiceDependencies(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        services.RegisterUseCases();
        services.RegisterUserDataClient();
        services.RegisterUserDataAdapter();
        
        services.AddScoped<IUserTaskFactory, UserTaskFactory>();
        
        services.RegisterDomainPersistence(configuration);
        services.RegisterMessageBroker(configuration);
        services.RegisterOutboxProcessor(configuration);
        
        return services;
    }
    
    internal static void AddOpenTelemetry(this WebApplicationBuilder webApplicationBuilder)
    {
        webApplicationBuilder.Services
            .AddOpenTelemetry()
            .ConfigureResource(
                resource => resource
                    .AddService(
                        serviceName: "user-task-api",
                        serviceVersion: typeof(Program).Assembly.GetName().Version?.ToString(),
                        serviceInstanceId: Environment.MachineName
                    )
            )
            .WithTracing(
                tracing => tracing
                    .AddAspNetCoreInstrumentation()
                    .AddHttpClientInstrumentation()
                    .AddEntityFrameworkCoreInstrumentation(
                        opt =>
                        {
                            opt.SetDbStatementForText = true;
                            opt.EnrichWithIDbCommand = (
                                activity,
                                command
                            ) =>
                            {
                                activity.SetTag(
                                    "db.statement",
                                    command.CommandText
                                );
                            };
                        }
                    )
                    .AddOtlpExporter()
            ) // Юзаем Jaeger
            .WithMetrics(
                metrics => metrics
                    .AddAspNetCoreInstrumentation()
                    .AddHttpClientInstrumentation()
                    .AddRuntimeInstrumentation()
                    .AddProcessInstrumentation()
                    .AddOtlpExporter()
            );

        // Для логов (опционально)
        webApplicationBuilder.Logging.AddOpenTelemetry(
            logging =>
            {
                logging.IncludeFormattedMessage = true;
                logging.IncludeScopes = true;
                logging.ParseStateValues = true;
                logging.AddOtlpExporter();
            }
        );
    }
}
