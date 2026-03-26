using UserTaskManagement.Domain.Models.UserTask;
using Xunit;

namespace UserTaskManagement.Tests.RulesTests;

public partial class IsAvailableForChangingStatusRuleTests
{
    public static class Constants
    {
        public const long ValidTaskId = 5001L;
        public const long ValidUserId = 1001L;
        public const string ValidTitle = "Title test";
        public const string ValidDescription = "Description test";
        
        public static readonly DateTime FixedCreatedAt = new(2024, 1, 15, 10, 30, 0, DateTimeKind.Utc);
        public static readonly DateTime FixedUpdatedAt = new(2024, 1, 16, 14, 45, 0, DateTimeKind.Utc);

        // Фабрика доменной задачи
        public static UserTask CreateUserTask(
            long id = ValidTaskId,
            long userId = ValidUserId,
            UserTaskStatus status = UserTaskStatus.New,
            string title = ValidTitle,
            string description = ValidDescription,
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
    }
    
    public static TheoryData<UserTaskStatus, UserTaskStatus, bool> GetValidTransitionsData() => new()
    {
        { UserTaskStatus.New, UserTaskStatus.InProgress, true },
        { UserTaskStatus.InProgress, UserTaskStatus.Completed, true },
    };

    public static TheoryData<UserTaskStatus, UserTaskStatus> GetInvalidTransitionsData() 
        => new()
        {
            { UserTaskStatus.New, UserTaskStatus.Completed },
            { UserTaskStatus.InProgress, UserTaskStatus.New },
            { UserTaskStatus.Completed, UserTaskStatus.New },
            { UserTaskStatus.Completed, UserTaskStatus.InProgress },
        };

    public static TheoryData<UserTaskStatus> GetSameStatusData() 
        =>
        [
            UserTaskStatus.New,
            UserTaskStatus.InProgress,
            UserTaskStatus.Completed
        ];
}
