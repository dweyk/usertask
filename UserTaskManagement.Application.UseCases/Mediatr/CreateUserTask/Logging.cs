using Microsoft.Extensions.Logging;

namespace UserTaskManagement.Application.UseCases.Mediatr.CreateUserTask;

public static partial class CreateUserTaskUseCase
{
    [LoggerMessage(
        LogLevel.Error,
        Message = "Не удалось создать юзер-таску reason={Reason}")]
    private static partial void LogCreatingUserTaskFailed(this ILogger logger, string reason);
    
    [LoggerMessage(
        LogLevel.Error,
        Message = "Не удалось найти юзера id={Id} reason={Reason}")]
    private static partial void LogFindUserInfoFailed(this ILogger logger, long id, string reason);
}
