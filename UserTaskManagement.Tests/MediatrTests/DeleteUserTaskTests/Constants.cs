using UserTaskManagement.Application.UseCases.Mediatr.DeleteUserTask;
using UserTaskManagement.Domain.Models.UserTask;
using UserTaskManagement.Domain.SeedWorks;
using Xunit;

namespace UserTaskManagement.Tests.MediatrTests.DeleteUserTaskTests;

public partial class DeleteUserTaskTests
{
    public static class Constants
    {
        public const long ValidUserTaskId = 5001L;
        public const long ValidUserId = 1001L;
        public const string ValidTitle = "Title test";
        public const string ValidDescription = "Description test";

        public const string ErrorUserTaskIdInvalid = "Идентификатор задачи должен быть больше 0";
        
        public static UserTask CreateUserTask(
            long id = ValidUserTaskId,
            long userId = ValidUserId
        )
        {
            return new UserTask(
                id,
                userId,
                version: default,
                ValidTitle,
                ValidDescription,
                UserTaskStatus.New,
                SysTime.UtcNow(),
                SysTime.UtcNow()
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

    public static DeleteUserTaskUseCase.Command CreateValidCommand()
    {
        return new DeleteUserTaskUseCase.Command(UserTaskId: Constants.ValidUserTaskId);
    }
}