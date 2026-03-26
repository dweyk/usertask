using FluentValidation;

namespace UserTaskManagement.Application.UseCases.Mediatr.CreateUserTask;

public static partial class CreateUserTaskUseCase
{
    /// <summary>
    /// Валидатор для команды создания задачи
    /// </summary>
    public sealed class CommandValidator : AbstractValidator<Command>
    {
        public CommandValidator()
        {
            RuleFor(x => x.UserId)
                .GreaterThan(0)
                .WithMessage("Идентификатор пользователя должен быть больше 0");

            RuleFor(x => x.Title)
                .NotEmpty()
                .WithMessage("Заголовок задачи не может быть пустым")
                .MaximumLength(200)
                .WithMessage("Заголовок задачи не должен превышать 200 символов");

            RuleFor(x => x.Description)
                .NotEmpty()
                .WithMessage("Описание задачи не может быть пустым")
                .MaximumLength(2000)
                .WithMessage("Описание задачи не должно превышать 2000 символов");
        }
    }
}
