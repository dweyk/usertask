using Microsoft.Extensions.DependencyInjection;
using UserTaskManagement.Application.DrivenPorts.UserData;

namespace UserTaskManagement.DrivenAdapters.UserData;

public static class Registrar
{
    public static IServiceCollection RegisterUserDataAdapter(
        this IServiceCollection services)
    {
        services.AddScoped<IUserDataPort, UserDataAdapter>();
        
        return services;
    }
}
