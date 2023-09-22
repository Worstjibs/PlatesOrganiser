using FluentValidation;
using MediatR;
using PlatesOrganiser.Domain.Shared;

namespace PlatesOrganiser.Application.Behaviours;

internal class ValidationPipelineBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
    where TResponse : Result
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public ValidationPipelineBehaviour(IEnumerable<IValidator<TRequest>> validators)
    {
        _validators = validators;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var validationTasks = _validators.Select(x => x.ValidateAsync(request, cancellationToken));

        var validationResults = await Task.WhenAll(validationTasks);

        var failures = validationResults
                                .SelectMany(x => x.Errors)
                                .Where(x => x != null)
                                .Select(x => x.ErrorMessage)
                                .ToArray();

        if (failures.Length > 0)
            return CreateResult(failures);

        return await next();
    }

    private TResponse CreateResult(string[] messages)
    {
        if (!typeof(TResponse).IsGenericType)
            return (Result.Failure(Error.Bad, messages.First()) as TResponse)!;

        var failureMethod = typeof(Result)
                                    .GetMethod(nameof(Result.Failure), 1, [typeof(Error), typeof(string)])!
                                    .MakeGenericMethod(typeof(TResponse).GenericTypeArguments[0]);

        var result = failureMethod.Invoke(null, [Error.Bad, messages.First()]);

        return (result as TResponse)!;
    }
}
