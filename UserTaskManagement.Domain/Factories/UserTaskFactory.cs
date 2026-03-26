using FluentResults;
using UserTaskManagement.Domain.Models.UserTask;
using UserTaskManagement.Domain.SeedWorks;

namespace UserTaskManagement.Domain.Factories;

/// <inheritdoc/>
public class UserTaskFactory : IUserTaskFactory
{
    public Result<UserTask> CreateUserTask(
        long userId,
        string title,
        string description)
    {
        var utcNow = SysTime.UtcNow();

        var userTask = new UserTask(
            id: default,
            userId: userId,
            version: default,
            title: title,
            description: description,
            status: UserTaskStatus.New,
            createdAt: utcNow,
            updatedAt: utcNow);

        return userTask;
    }
}
