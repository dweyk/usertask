using Microsoft.Extensions.DependencyInjection;

namespace UserTaskManagement.Client.UserData;

public static class Registrar
{
    public static IServiceCollection RegisterUserDataClient(
        this IServiceCollection services)
    {
        services.AddScoped<IUserDataClient, UserDataClient>();
        
        return services;
    }
}
