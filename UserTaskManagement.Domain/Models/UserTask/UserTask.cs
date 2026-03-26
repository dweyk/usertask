using FluentResults;
using UserTaskManagement.Domain.Rules.UserTask;
using UserTaskManagement.Domain.SeedWorks;
using UserTaskManagement.Domain.SeedWorks.Aggregate.Impl;

namespace UserTaskManagement.Domain.Models.UserTask;

public sealed partial class UserTask : AggregateRoot
{
    public UserTask(
        long id,
        long userId,
        uint version,
        string title,
        string description,
        UserTaskStatus status,
        DateTime createdAt,
        DateTime updatedAt) : base(id, version)
    {
        UserId = userId;
        Title = title;
        Description = description;
        Status = status;
        CreatedAt = createdAt;
        UpdatedAt = updatedAt;
    }
    
    /// <summary>
    /// Название задачи
    /// </summary>
    public string Title { get; private set; }
    
    /// <summary>
    /// Описание задачи
    /// </summary>
    public string Description { get; private set; }
    
    /// <summary>
    /// Статус задачи
    /// </summary>
    public UserTaskStatus Status { get; private set; }
    
    /// <summary>
    /// Дата создания
    /// </summary>
    public DateTime CreatedAt { get; private set; }
    
    /// <summary>
    /// Дата обновления
    /// </summary>
    public DateTime UpdatedAt { get; private set; }
    
    /// <summary>
    /// Идентификатор пользователя, на которого назначили задачу
    /// </summary>
    public long UserId { get; private set; }

    public Result MoveStatus(UserTaskStatus status)
    {
        if (Status == status)
        {
            return Result.Fail("Юзер таска в указанном статусе");
        }
        
        var canMoveStatus = new IsAvailableForChangingStatusRule(status)
            .Apply(this);

        if (canMoveStatus.IsFailed)
        {
            return canMoveStatus;
        }
        
        Status = status;
        UpdatedAt = SysTime.UtcNow();

        return Result.Ok();
    }
}
