namespace UserTaskManagement;

/// <summary>
/// Регистрация сваггера
/// </summary>
internal static class SwaggerRegistrar
{
    internal static IServiceCollection AddSwagger(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
            {
                Title = "User Task Management API",
                Version = "v1",
                Description = "API для управления пользовательскими задачами"
            });
            
            c.EnableAnnotations();
            c.CustomSchemaIds(type =>
            {
                var fullName = type.FullName;
                
                return fullName != null
                    ? fullName.Replace("+", " ")
                    : type.Name;
            });
        });
    
        return services;
    }

    internal static void AddSwaggerUi(this WebApplication webApplication)
    {
        webApplication.UseSwagger();
        webApplication.UseSwaggerUI(c => 
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "UserTaskManagement API v1");
        });
    }
}
