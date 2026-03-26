using FluentResults;
using UserTaskManagement.Domain.Models.UserTask;

namespace UserTaskManagement.Domain.Factories;

/// <summary>
/// Фабрика создания пользовательской задачи
/// </summary>
public interface IUserTaskFactory
{
    /// <summary>
    /// Создать пользовательскую задачу
    /// </summary>
    /// <param name="userId">Идентификатор пользователя</param>
    /// <param name="title">Название задачи</param>
    /// <param name="description">Описание задачи</param>
    public Result<UserTask> CreateUserTask(
        long userId,
        string title,
        string description);
}