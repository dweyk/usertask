using FluentResults;
using MediatR;

namespace UserTaskManagement.Application.UseCases.Mediatr.CreateUserTask;

/// <summary>
/// UseCase для создания задачи пользователя
/// </summary>
public static partial class CreateUserTaskUseCase
{
    /// <summary>
    /// Команда на создание задачи
    /// </summary>
    /// <param name="UserId">Идентификатор пользователя, создающего задачу</param>
    /// <param name="Title">Заголовок задачи</param>
    /// <param name="Description">Описание задачи</param>
    public sealed record Command(
        long UserId,
        string Title,
        string Description
    ) : IRequest<Result<CommandResult>>;

    /// <summary>
    /// Результат выполнения команды создания задачи
    /// </summary>
    /// <param name="UserTaskId">Идентификатор созданной задачи</param>
    public sealed record CommandResult(long UserTaskId);
}
