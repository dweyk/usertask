using FluentResults;
using MediatR;
using UserTaskManagement.Domain.Models.UserTask;

namespace UserTaskManagement.Application.UseCases.Mediatr.UpdateUserTask;

/// <summary>
/// UseCase для обновления статуса задачи пользователя
/// </summary>
public static partial class UpdateUserTaskUseCase
{
    /// <summary>
    /// Команда на обновление статуса задачи
    /// </summary>
    /// <param name="UserTaskId">Идентификатор обновляемой задачи</param>
    /// <param name="Status">Новый статус задачи</param>
    public sealed record Command(
        long UserTaskId,
        UserTaskStatus Status
    ) : IRequest<Result>;
}
