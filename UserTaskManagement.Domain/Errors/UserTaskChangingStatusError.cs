using FluentResults;
using UserTaskManagement.Domain.Models.UserTask;

namespace UserTaskManagement.Domain.Errors;

/// <summary>
/// Доменная ошибка перехода статуса задачи
/// </summary>
public sealed class UserTaskChangingStatusError : Error
{
    public UserTaskChangingStatusError(UserTaskStatus oldStatus, UserTaskStatus newStatus)
    {
        Message = $"Невозможно поменять статус задачи с {oldStatus} на {newStatus}";
    }
}
