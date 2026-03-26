using System.Text;
using FluentResults;

namespace UserTaskManagement.Application.Extensions;

public static class FluentResultExtensions
{
    /// <summary>
    /// Преобразовать <see cref="Error"/> в строку
    /// </summary>
    public static string ToErrorString(this ResultBase result)
    {
        if (result.IsSuccess)
        {
            throw new ArgumentException("Невалидный статус результата", nameof(result));
        }
        
        var errors = result.Errors;

        if (!errors.Any<IError>())
        {
            return "Произошла неожиданная ошибка";
        }

        var sb = new StringBuilder();
        Stringify(sb, errors);

        static void Stringify(StringBuilder sb, IReadOnlyList<IError> errors)
        {
            foreach (var error in errors)
            {
                sb.Append(error.Message);

                if (error.Reasons.Any<IError>())
                {
                    sb.Append(": ");
                    Stringify(sb, error.Reasons);
                }
            }
        }

        return sb.ToString().TrimEnd();
    }
}
