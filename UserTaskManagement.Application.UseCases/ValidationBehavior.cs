using FluentResults;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;

namespace UserTaskManagement.Application.UseCases;

/// <summary>
/// Поведение для валидации запросов и команд
/// </summary>
public class ValidationBehavior<TRequest, TResponse>
    : IPipelineBehavior<TRequest, TResponse>
    where TResponse : ResultBase, new()
    where TRequest : notnull
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;
    private readonly ILogger<ValidationBehavior<TRequest, TResponse>> _logger;

    public ValidationBehavior(
        IEnumerable<IValidator<TRequest>> validators,
        ILogger<ValidationBehavior<TRequest, TResponse>> logger
    )
    {
        _validators = validators;
        _logger = logger;
    }

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken
    )
    {
        if (_validators.Any())
        {
            var context = new ValidationContext<TRequest>(request);

            var results = await Task.WhenAll(
                _validators.Select(x => x.ValidateAsync(context, cancellationToken))
            );

            var validationFailures = results.SelectMany(x => x.Errors)
                .Where(x => x is not null)
                .ToList();
            
            if (validationFailures.Count != 0)
            {
                var reason = string.Join("; ", validationFailures.Select(x => x.ErrorMessage));
                
                _logger.LogWarning("Ошибка валидации. {reason}", reason);
                
                var result = new TResponse();
                
                result.Reasons.Add(new Error("Ошибка валидации. " + reason));
                
                return result;
            }
        }

        return await next();
    }
}