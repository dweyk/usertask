using UserTaskManagement.Application.UseCases.Mediatr.UpdateUserTask;
using UserTaskManagement.Domain.Models.UserTask;
using Xunit;

namespace UserTaskManagement.Tests.MediatrTests.UpdateUserTaskTests;

public partial class UpdateUserTaskTests
{
    public static class Constants
    {
        public const long ValidUserTaskId = 5001L;
        public const long ValidUserId = 1001L;
        public const string ValidTitle = "Title test";
        public const string ValidDescription = "Description test";
        
        public const string ErrorUserTaskIdInvalid = "Идентификатор задачи должен быть больше 0";
        public const string Error = "Не удалось обновить задачу";
        
        public static readonly DateTime FixedCreatedAt = new(2024, 1, 15, 10, 30, 0, DateTimeKind.Utc);
        public static readonly DateTime FixedUpdatedAt = new(2024, 1, 16, 14, 45, 0, DateTimeKind.Utc);

        // Фабрика доменной задачи
        public static UserTask CreateUserTask(
            long id = ValidUserTaskId,
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

    public static TheoryData<long, bool, string> GetUserTaskIdData() => new()
    {
        { Constants.ValidUserTaskId, true, "" },
        { long.MaxValue, true, "" },
        { 1, true, "" },
        { 0, false, Constants.ErrorUserTaskIdInvalid },
        { -1, false, Constants.ErrorUserTaskIdInvalid },
        { long.MinValue, false, Constants.ErrorUserTaskIdInvalid },
    };

    public static TheoryData<UserTaskStatus, UserTaskStatus, bool> GetStatusTransitionValidData() 
        => new()
        {
            { UserTaskStatus.New, UserTaskStatus.InProgress, true },
            { UserTaskStatus.InProgress, UserTaskStatus.Completed, true },
            
        };
    
    public static TheoryData<UserTaskStatus, UserTaskStatus, bool> GetStatusTransitionInvalidData() 
        => new()
        {
            { UserTaskStatus.InProgress, UserTaskStatus.New, false },
            { UserTaskStatus.Completed, UserTaskStatus.InProgress, false },
        };

    public static UpdateUserTaskUseCase.Command CreateValidCommand(
        long userTaskId = Constants.ValidUserTaskId,
        UserTaskStatus status = UserTaskStatus.InProgress)
    {
        return new UpdateUserTaskUseCase.Command(
            UserTaskId: userTaskId,
            Status: status
        );
    }
}
