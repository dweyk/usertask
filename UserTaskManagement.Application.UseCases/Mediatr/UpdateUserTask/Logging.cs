using Microsoft.Extensions.Logging;

namespace UserTaskManagement.Application.UseCases.Mediatr.UpdateUserTask;

public static partial class UpdateUserTaskUseCase
{
    [LoggerMessage(
        LogLevel.Error,
        Message = "Не удалось обновить юзер-таску reason={Reason}")]
    private static partial void LogUpdateUserTaskFailed(this ILogger logger, string reason);
    
    [LoggerMessage(
        LogLevel.Error,
        Message = "Не удалось найти юзер-таску id={Id}")]
    private static partial void LogFindUserTaskFailed(this ILogger logger, long id);
}
