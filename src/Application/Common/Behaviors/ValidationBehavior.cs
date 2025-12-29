using FluentValidation;
using MediatR;
using PaySplit.Application.Common.Results;

namespace PaySplit.Application.Common.Behaviors;

public sealed class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
    {
        _validators = validators;
    }

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        if (_validators.Any())
        {
            var context = new ValidationContext<TRequest>(request);
            var failures = (await Task.WhenAll(
                    _validators.Select(v => v.ValidateAsync(context, cancellationToken))))
                .SelectMany(result => result.Errors)
                .Where(f => f is not null)
                .ToList();

            if (failures.Count != 0)
            {
                var message = string.Join("; ", failures.Select(f => f.ErrorMessage));
                var responseType = typeof(TResponse);

                if (responseType == typeof(Result))
                {
                    return (TResponse)(object)Result.Failure(message);
                }

                if (responseType.IsGenericType && responseType.GetGenericTypeDefinition() == typeof(Result<>))
                {
                    var failureMethod = responseType.GetMethod("Failure", new[] { typeof(string) });
                    if (failureMethod is not null)
                    {
                        return (TResponse)failureMethod.Invoke(null, new object[] { message })!;
                    }
                }

                throw new ValidationException(message, failures);
            }
        }

        return await next();
    }
}
