using FluentResults;
using MediatR;

namespace UserTaskManagement.Application.UseCases.Mediatr.GetUserTasks;

/// <summary>
/// UseCase для получения списка задач пользователей
/// </summary>
public static partial class GetUserTasksUseCase
{
    /// <summary>
    /// Запрос на получение списка задач
    /// </summary>
    public sealed record Query : IRequest<Result<QueryResult>>;

    /// <summary>
    /// Результат выполнения запроса на получение списка задач
    /// </summary>
    /// <param name="UserTasks">Коллекция задач пользователей</param>
    public sealed record QueryResult(IReadOnlyCollection<UserTaskItem> UserTasks);
    
    /// <summary>
    /// Задача пользователя
    /// </summary>
    /// <param name="TaskId">Идентификатор задачи</param>
    /// <param name="UserId">Идентификатор пользователя</param>
    /// <param name="Status">Статус задачи</param>
    /// <param name="CreatedAt">Дата создания задачи</param>
    /// <param name="UpdatedAt">Дата последнего обновления задачи</param>
    /// <param name="Title">Заголовок задачи</param>
    /// <param name="Description">Описание задачи</param>
    public sealed record UserTaskItem(
        long TaskId,
        long UserId,
        string Status,
        DateTime CreatedAt,
        DateTime UpdatedAt,
        string Title,
        string Description
    );
}
