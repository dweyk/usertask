using FluentResults;

namespace UserTaskManagement.Client.UserData;

/// <summary>
/// Клиент для получения пользователей
/// </summary>
public interface IUserDataClient
{
    /// <summary>
    /// Получить информацию по пользователю
    /// </summary>
    /// <param name="request">Запрос для получения юзера</param>
    Task<Result<GetUserInfoResponse>> GetUserInfo(GetUserInfoRequest request);
}
