namespace UserTaskManagement.Application.Errors;

public sealed class NotFoundError : ApplicationError
{
    public NotFoundError()
    {
        Message = "Не найдено";
    }
}
