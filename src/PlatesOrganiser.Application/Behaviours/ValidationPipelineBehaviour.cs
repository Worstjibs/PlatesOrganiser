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
        var validationTasks = _validators.Select(x => x.ValidateAsync(new ValidationContext<TRequest>(request), cancellationToken));

        var validationResults = await Task.WhenAll(validationTasks);

        var failures = validationResults
                                .SelectMany(x => x.Errors)
                                .Where(x => x != null)
                                .Select(x => x.ErrorMessage)
                                .ToArray();

        if (failures.Length > 0)
            return CreateResult<TResponse>(failures);

        return await next();
    }

    private TResult CreateResult<TResult>(string[] messages) where TResult : Result
    {
        if (typeof(TResult) != typeof(Result<>))
            return (Result.Failure(Error.Bad, messages.First()) as TResult)!;

        //var result = typeof(Result<>)
        //                    .GetGenericTypeDefinition()
        //                    .MakeGenericType(typeof(TResult).GenericTypeArguments[0])
        //                    .GetMethod("Failure");

        var failureMethod = typeof(Result)
                                    .GetMethod(nameof(Result.Failure))!
                                    .MakeGenericMethod(typeof(TResult).GenericTypeArguments[0]);

        var result = failureMethod.Invoke(null, new object[] { Error.Bad });

        return (result as TResult)!;
    }
}
