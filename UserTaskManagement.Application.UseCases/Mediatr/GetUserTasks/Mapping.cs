using UserTaskManagement.Domain.Models.UserTask;

namespace UserTaskManagement.Application.UseCases.Mediatr.GetUserTasks;

public static partial class GetUserTasksUseCase
{
    public static QueryResult ToQueryResult(IOrderedEnumerable<UserTask> orderedUserTask)
    {
        var result = orderedUserTask.Select(
            x => new UserTaskItem(
                TaskId: x.Id,
                UserId: x.UserId,
                Status: x.Status.ToString(),
                CreatedAt: x.CreatedAt,
                UpdatedAt: x.UpdatedAt,
                Title: x.Title,
                Description: x.Description
            )
        ).ToList();
            
        return new QueryResult(result);
    }
}
