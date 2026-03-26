namespace UserTaskManagement.Application.DrivenPorts.UserData.Models;

/// <summary>
/// Данные юзера
/// </summary>
/// <param name="UserId">Идентификатор юзера</param>
/// <param name="Name">Имя юзера</param>
public sealed record UserData(
    long UserId,
    string Name
);
