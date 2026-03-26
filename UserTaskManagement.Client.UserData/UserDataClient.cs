using FluentResults;
using UserTaskManagement.Application.Errors;

namespace UserTaskManagement.Client.UserData;

/// <summary>
/// Клиент пользователей, здесь может быть запрос через grpc, http
/// но в рамках тестового не указана задача реализовать пользователей, поэтому замокаем ответ
/// </summary>
internal sealed class UserDataClient : IUserDataClient
{
    private readonly IReadOnlyDictionary<long, GetUserInfoResponse> _userInfos = CreateUserInfos();

    /// <inheritdoc/>
    public async Task<Result<GetUserInfoResponse>> GetUserInfo(GetUserInfoRequest request)
    {
        await Task.CompletedTask;

        try
        {
            if (_userInfos.TryGetValue(request.UserId, out var userInfo))
            {
                return userInfo;
            }

            return Result.Fail(new NotFoundError());
        }
        catch (Exception e) // по идее тут должна быть сетевая ошибка
        {
            return Result.Fail(e.Message);
        }
    }

    /// <summary>
    /// Мок юзеров
    /// </summary>
    private static IReadOnlyDictionary<long, GetUserInfoResponse> CreateUserInfos()
    {
        return new Dictionary<long, GetUserInfoResponse>()
        {
            [1] = new(UserId: 1, "Vitaliy"),
            [2] = new(UserId: 2, "Nikolay"),
            [3] = new(UserId: 3, "Ksenia"),
            [4] = new(UserId: 4, "Irina"),
            [5] = new(UserId: 5, "Yan"),
            [6] = new(UserId: 6, "Roma"),
            [7] = new(UserId: 7, "Inna"),
            [8] = new(UserId: 8, "Varya"),
            [9] = new(UserId: 9, "Maksim"),
            [10] = new(UserId: 10, "Aleksey"),
        };
    }
}
