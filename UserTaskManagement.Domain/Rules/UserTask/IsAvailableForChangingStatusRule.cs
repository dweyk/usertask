using FluentResults;
using UserTaskManagement.Domain.Errors;
using UserTaskManagement.Domain.Models.UserTask;
using UserTaskManagement.Domain.SeedWorks.EntityRule;

namespace UserTaskManagement.Domain.Rules.UserTask;

/// <summary>
/// Правило перехода статуса пользовательской задачи
/// </summary>
public class IsAvailableForChangingStatusRule : IEntityRule<Models.UserTask.UserTask>
{
    private readonly UserTaskStatus _newStatus;
    
    public IsAvailableForChangingStatusRule(UserTaskStatus newStatus)
    {
        _newStatus = newStatus;
    }
    
    private static readonly Dictionary<UserTaskStatus, HashSet<UserTaskStatus>> ValidTransitions = new()
    {
        [UserTaskStatus.New] = [UserTaskStatus.InProgress],
        [UserTaskStatus.InProgress] = [UserTaskStatus.Completed]
    };
        
    /// <inheritdoc/>
    public Result Apply(Models.UserTask.UserTask entity)
    {
        var currentStatus = entity.Status;

        if (currentStatus == _newStatus)
        {
            return Result.Fail(new UserTaskChangingStatusError(currentStatus, _newStatus));
        }

        if (ValidTransitions.TryGetValue(currentStatus, out var transitions) &&
            transitions.Contains(_newStatus))
        {
            return Result.Ok();
        }
        
        return Result.Fail(new UserTaskChangingStatusError(currentStatus, _newStatus));
    }
}
