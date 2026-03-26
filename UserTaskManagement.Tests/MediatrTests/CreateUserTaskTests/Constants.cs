using UserTaskManagement.Application.DrivenPorts.UserData.Models;
using UserTaskManagement.Application.UseCases.Mediatr.CreateUserTask;
using UserTaskManagement.Domain.Models.UserTask;
using UserTaskManagement.Domain.SeedWorks;
using Xunit;

namespace UserTaskManagement.Tests.MediatrTests.CreateUserTaskTests;

public partial class CreateUserTaskTests
{
    public static class Constants
    {
        public const long ValidUserId = 1001L;
        public const long CreatedUserTaskId = 5001L;
        public const string ValidTitle = "Title test";
        public const string ValidDescription = "Description test";
        
        public const int TitleMaxLength = 200;
        public const int DescriptionMaxLength = 2000;
        
        public const string ErrorUserIdInvalid = "Идентификатор пользователя должен быть больше 0";
        public const string ErrorTitleEmpty = "Заголовок задачи не может быть пустым";
        public const string ErrorTitleTooLong = "Заголовок задачи не должен превышать 200 символов";
        public const string ErrorDescriptionTooLong = "Описание задачи не должно превышать 2000 символов";
        public const string ErrorDescriptionEmpty = "Описание задачи не может быть пустым";
        public const string Error = "Не удалось создать задачу";
    }

    public static TheoryData<long, bool, string> GetUserIdData() => new()
    {
        { Constants.ValidUserId, true, "" },
        { long.MaxValue, true, "" },
        { 0, false, Constants.ErrorUserIdInvalid },
        { -1, false, Constants.ErrorUserIdInvalid },
        { long.MinValue, false, Constants.ErrorUserIdInvalid },
    };

    public static TheoryData<string, bool, string> GetTitleData()
    {
        return new TheoryData<string, bool, string>
        {
            { "Task", true, "" },
            { new string('A', Constants.TitleMaxLength), true, "" },
            { "", false, Constants.ErrorTitleEmpty },
            { "   ", false, Constants.ErrorTitleEmpty },
            { null!, false, Constants.ErrorTitleEmpty },
            { new string('A', Constants.TitleMaxLength + 1), false, Constants.ErrorTitleTooLong },
        };
    }

    public static TheoryData<string, bool, string> GetDescriptionData() => new()
    {
        { "Описание", true, "" },
        { new string('B', Constants.DescriptionMaxLength), true, "" },
        { "", false, Constants.ErrorDescriptionEmpty },
        { null!, false, Constants.ErrorDescriptionEmpty },
        { new string('B', Constants.DescriptionMaxLength + 1), false, Constants.ErrorDescriptionTooLong },
    };
    

    public static CreateUserTaskUseCase.Command CreateValidCommand()
    {
        return new CreateUserTaskUseCase.Command(
            UserId: Constants.ValidUserId,
            Title: Constants.ValidTitle,
            Description: Constants.ValidDescription
        );
    }
    
    public static UserTask CreateUserTask(CreateUserTaskUseCase.Command command)
    {
        var userTask = new UserTask(
            Constants.CreatedUserTaskId,
            command.UserId,
            version: default,
            command.Title,
            command.Description,
            UserTaskStatus.New,
            SysTime.UtcNow(),
            SysTime.UtcNow()
        );
        return userTask;
    }

    public static UserData CreateUserData(CreateUserTaskUseCase.Command command)
    {
        var userData = new UserData(
            command.UserId,
            "Vitaliy"
        );
        return userData;
    }
}
