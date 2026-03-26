using Microsoft.Extensions.Logging;

namespace UserTaskManagement.Application.UseCases.Mediatr.DeleteUserTask;

public static partial class DeleteUserTaskUseCase
{
    [LoggerMessage(
        LogLevel.Error,
        Message = "Не удалось найти юзер-таску id={Id}")]
    private static partial void LogFindUserTaskFailed(this ILogger logger, long id);
}
