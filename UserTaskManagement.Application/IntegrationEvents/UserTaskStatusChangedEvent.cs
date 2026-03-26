using UserTaskManagement.Domain.SeedWorks;

namespace UserTaskManagement.Application.IntegrationEvents;

/// <summary>
/// Событие при изменении статуса задачи пользователя
/// </summary>
public sealed class UserTaskStatusChangedEvent : BaseEvent
{
    private UserTaskStatusChangedEvent(
        Guid eventId,
        long userId,
        long taskId,
        string oldStatus,
        string newStatus,
        DateTime updatedAt
    ) : base(nameof(UserTaskStatusChangedEvent), SysTime.UtcNow())
    {
        EventId = eventId;
        UserId = userId;
        TaskId = taskId;
        OldStatus = oldStatus;
        NewStatus = newStatus;
        UpdatedAt = updatedAt;
    }

    /// <summary>
    /// Идентификатор события
    /// </summary>
    public Guid EventId { get; }
    
    /// <summary>
    /// Идентификатор пользователя, которому принадлежит задача
    /// </summary>
    public long UserId { get; }
    
    /// <summary>
    /// Идентификатор задачи
    /// </summary>
    public long TaskId { get; }

    /// <summary>
    /// Предыдущий статус задачи
    /// </summary>
    public string OldStatus { get; }

    /// <summary>
    /// Новый статус задачи
    /// </summary>
    public string NewStatus { get; }
    
    /// <summary>
    /// Время изменения статуса
    /// </summary>
    public DateTime UpdatedAt { get; }
    
    /// <summary>
    /// Статик фабрика события UserTaskStatusChangedEvent
    /// </summary>
    /// <param name="taskId">Идентификатор задачи</param>
    /// <param name="userId">Идентификатор пользователя</param>
    /// <param name="oldStatus">Предыдущий статус</param>
    /// <param name="newStatus">Новый статус</param>
    /// <param name="updatedAt">Время изменения</param>
    public static UserTaskStatusChangedEvent Create(
        long taskId,
        long userId,
        string oldStatus,
        string newStatus,
        DateTime updatedAt
    )
    {
        return new UserTaskStatusChangedEvent(
            eventId: Guid.NewGuid(),
            userId: userId,
            taskId: taskId,
            oldStatus: oldStatus,
            newStatus: newStatus,
            updatedAt: updatedAt
        );
    }
}
