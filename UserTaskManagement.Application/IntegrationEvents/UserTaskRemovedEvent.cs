using UserTaskManagement.Domain.SeedWorks;

namespace UserTaskManagement.Application.IntegrationEvents;

/// <summary>
/// Событие при удалении задачи пользователя
/// </summary>
public sealed class UserTaskRemovedEvent : BaseEvent
{
    private UserTaskRemovedEvent(
        Guid eventId,
        long taskId,
        long userId
    ) : base(nameof(UserTaskRemovedEvent), SysTime.UtcNow())
    {
        EventId = eventId;
        UserId = userId;
        TaskId = taskId;
    }
    
    /// <summary>
    /// Идентификатор события
    /// </summary>
    public Guid EventId { get; }
    
    /// <summary>
    /// Идентификатор пользователя, которому принадлежала задача
    /// </summary>
    public long UserId { get; }
    
    /// <summary>
    /// Идентификатор удаленной задачи
    /// </summary>
    public long TaskId { get; }
    
    /// <summary>
    /// Статик фабрика события UserTaskRemovedEvent
    /// </summary>
    /// <param name="taskId">Идентификатор удаленной задачи</param>
    /// <param name="userId">Идентификатор пользователя</param>
    public static UserTaskRemovedEvent Create(long taskId, long userId)
    {
        return new UserTaskRemovedEvent(
            eventId: Guid.NewGuid(),
            userId: userId,
            taskId: taskId
        );
    }
}
