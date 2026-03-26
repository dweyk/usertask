using FluentResults;

namespace UserTaskManagement.Application.DrivenPorts.UserData;

public interface IUserDataPort
{
    /// <summary>
    /// Получить информацию о пользователе
    /// </summary>
    /// <param name="userId">Идентификатор пользователя</param>
    Task<Result<Models.UserData>> GetUserData(long userId);
}
