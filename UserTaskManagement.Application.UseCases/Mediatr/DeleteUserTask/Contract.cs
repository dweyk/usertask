using FluentResults;
using MediatR;

namespace UserTaskManagement.Application.UseCases.Mediatr.DeleteUserTask;

/// <summary>
/// UseCase для удаления задачи пользователя
/// </summary>
public static partial class DeleteUserTaskUseCase
{
    /// <summary>
    /// Команда на удаление задачи
    /// </summary>
    /// <param name="UserTaskId">Идентификатор удаляемой задачи</param>
    public sealed record Command(long UserTaskId) : IRequest<Result>;

    /// <summary>
    /// Результат выполнения команды удаления задачи
    /// </summary>
    /// <param name="UserTaskId">Идентификатор удаленной задачи</param>
    public sealed record CommandResult(long UserTaskId);
}
