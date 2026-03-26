using Microsoft.EntityFrameworkCore;
using UserTaskManagement;
using UserTaskManagement.DrivenAdapters.DomainModel;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.AddOpenTelemetry();

var services = builder.Services;

builder.Services.AddSwagger();

var configuration = builder.Configuration;

builder.Services.RegisterServiceDependencies(configuration);

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    // Ожидаем, пока база данных станет доступной (можно добавить логику повторных попыток)
    dbContext.Database.Migrate();
} 

app.AddSwaggerUi();

app.MapControllers();
app.UseHttpsRedirection();

app.Run();
