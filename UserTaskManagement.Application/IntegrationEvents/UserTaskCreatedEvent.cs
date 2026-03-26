using UserTaskManagement.Domain.SeedWorks;

namespace UserTaskManagement.Application.IntegrationEvents;

/// <summary>
/// Событие создании новой задачи пользователя
/// </summary>
public sealed class UserTaskCreatedEvent : BaseEvent
{
    private UserTaskCreatedEvent(
        Guid eventId,
        DateTime createAt,
        long taskId,
        long userId,
        string title,
        string description
    ) : base(nameof(UserTaskCreatedEvent), createAt)
    {
        EventId = eventId;
        CreateAt = createAt;
        TaskId = taskId;
        UserId = userId;
        Title = title;
        Description = description;
    }
    
    /// <summary>
    /// Идентификатор события
    /// </summary>
    public Guid EventId { get; }
    
    /// <summary>
    /// Идентификатор созданной задачи
    /// </summary>
    public long TaskId { get; }
    
    /// <summary>
    /// Идентификатор пользователя, создавшего задачу
    /// </summary>
    public long UserId { get; }
    
    /// <summary>
    /// Заголовок задачи
    /// </summary>
    public string Title { get; }
    
    /// <summary>
    /// Описание задачи
    /// </summary>
    public string Description { get; }
    
    /// <summary>
    /// Время создания задачи
    /// </summary>
    public DateTime CreateAt { get; }
    
    /// <summary>
    /// Статик фабрика события UserTaskCreatedEvent
    /// </summary>
    /// <param name="taskId">Идентификатор задачи</param>
    /// <param name="userId">Идентификатор пользователя</param>
    /// <param name="title">Заголовок задачи</param>
    /// <param name="description">Описание задачи</param>
    public static UserTaskCreatedEvent Create(
        long taskId,
        long userId,
        string title,
        string description
    )
    {
        return new UserTaskCreatedEvent(
            eventId: Guid.NewGuid(),
            createAt: SysTime.UtcNow(),
            taskId: taskId,
            userId: userId,
            title: title,
            description: description
        );
    }
}
