using FluentValidation;

namespace UserTaskManagement.Application.UseCases.Mediatr.UpdateUserTask;

public static partial class UpdateUserTaskUseCase
{
    /// <summary>
    /// Валидатор для команды обновления статуса задачи
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
