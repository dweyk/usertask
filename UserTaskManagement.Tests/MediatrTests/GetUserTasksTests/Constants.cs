using UserTaskManagement.Application.UseCases.Mediatr.GetUserTasks;
using UserTaskManagement.Domain.Models.UserTask;

namespace UserTaskManagement.Tests.MediatrTests.GetUserTasksTests;

public partial class GetUserTasksTests
{
    public static class Constants
    {
        public const long ValidTaskId1 = 5001L;
        public const long ValidTaskId2 = 5002L;
        public const long ValidUserId1 = 1001L;
        public const long ValidUserId2 = 1002L;
        public const string ValidTitle1 = "Title 1 test";
        public const string ValidTitle2 = "Title 2 test";
        public const string ValidDescription1 = "Description 1 test";
        public const string ValidDescription2 = "Description 2 test";
        
        public static readonly DateTime FixedCreatedAt = new(2024, 1, 15, 10, 30, 0, DateTimeKind.Utc);
        public static readonly DateTime FixedUpdatedAt = new(2024, 1, 16, 14, 45, 0, DateTimeKind.Utc);
        
        public static UserTask CreateUserTask(
            long id,
            long userId,
            string title,
            string description,
            UserTaskStatus status = UserTaskStatus.New,
            DateTime? createdAt = null,
            DateTime? updatedAt = null)
        {
            return new UserTask(
                id,
                userId,
                version: default,
                title,
                description,
                status,
                createdAt ?? FixedCreatedAt,
                updatedAt ?? FixedUpdatedAt
            );
        }
        
        public static GetUserTasksUseCase.UserTaskItem CreateUserTaskItem(
            long taskId,
            long userId,
            string title,
            string description,
            string status = "New",
            DateTime? createdAt = null,
            DateTime? updatedAt = null)
        {
            return new GetUserTasksUseCase.UserTaskItem(
                TaskId: taskId,
                UserId: userId,
                Status: status,
                CreatedAt: createdAt ?? FixedCreatedAt,
                UpdatedAt: updatedAt ?? FixedUpdatedAt,
                Title: title,
                Description: description
            );
        }
    }

    public static GetUserTasksUseCase.Query CreateValidQuery()
    {
        return new GetUserTasksUseCase.Query();
    }
}