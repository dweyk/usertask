using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using UserTaskManagement.Application.UseCases.Mediatr.CreateUserTask;

namespace UserTaskManagement.Application.UseCases;

public static class Registrar
{
    /// <summary>
    /// Зарегестрировать юз кейсы и валидацию контрактов
    /// </summary>
    public static IServiceCollection RegisterUseCases(
        this IServiceCollection services)
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Registrar).Assembly));
        
        services.AddValidatorsFromAssembly(typeof(CreateUserTaskUseCase.CommandValidator).Assembly);
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        
        return services;
    }
}
