namespace UserTaskManagement.Client.UserData;

/// <summary>
/// Запрос на получение юзера
/// </summary>
/// <param name="UserId">Идентификатор юзера</param>
public sealed record GetUserInfoRequest(long UserId);
