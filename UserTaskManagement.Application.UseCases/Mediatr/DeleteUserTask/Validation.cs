using FluentValidation;

namespace UserTaskManagement.Application.UseCases.Mediatr.DeleteUserTask;

public static partial class DeleteUserTaskUseCase
{
    /// <summary>
    /// Валидатор для команды удаления задачи
    /// </summary>
    public sealed class CommandValidator : AbstractValidator<Command>
    {
        public CommandValidator()
        {
            RuleFor(x => x.UserTaskId)
                .GreaterThan(0)
                .WithMessage("Идентификатор задачи должен быть больше 0");
        }
    }
}
