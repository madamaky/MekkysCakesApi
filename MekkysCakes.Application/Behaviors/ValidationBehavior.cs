using FluentValidation;
using MediatR;
using MekkysCakes.Shared.CommonResult;

namespace MekkysCakes.Application.Behaviors
{
    /// <summary>
    /// MediatR pipeline behavior that automatically validates requests using FluentValidation.
    /// If validation fails, the handler is never called — a Result.Fail is returned instead.
    /// 
    /// How it works:
    /// 1. MediatR calls this behavior BEFORE the actual handler
    /// 2. It looks for all IValidator<TRequest> implementations in the DI container
    /// 3. If validators exist, it runs them all against the request
    /// 4. If any validation errors are found, it returns Result.Fail with the errors
    /// 5. If no errors (or no validators), it calls next() to proceed to the handler
    /// </summary>

    // Without this class, you'd have to manually call validation inside every single handler

    public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
    {
        // IEnumerable because there could be MULTIPLE validators for the same request
        private readonly IEnumerable<IValidator<TRequest>> _validators;

        public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
        {
            _validators = validators;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            // If there are no validators for this request type, skip validation entirely
            if (!_validators.Any())
                return await next();

            // Run all validators and collect the results
            var validationContext = new ValidationContext<TRequest>(request);
            var validationResults = await Task.WhenAll(
                _validators.Select(v => v.ValidateAsync(validationContext, cancellationToken))
            );

            // Flatten all errors from all validators into a single list
            var errors = validationResults
                .SelectMany(result => result.Errors)
                .Where(failure => failure is not null)
                .Select(failure => Error.Validation(failure.PropertyName, failure.ErrorMessage))
                .ToList();

            // If there are validation errors, return a failed result WITHOUT calling the handler
            if (errors.Any())
            {
                // We need to create results back dynamically because TResponse could be:
                //   - Result<bool>
                //   - Result<ProductDTO>
                //   - etc.

                // Check if TResponse is a generic Result<T>
                var responseType = typeof(TResponse);

                if (responseType.IsGenericType && responseType.GetGenericTypeDefinition() == typeof(Result<>))
                {
                    // Result<T> — call Result<T>.Fail(errors)
                    var failMethod = responseType.GetMethod(nameof(Result.Fail), new[] { typeof(List<Error>) });
                    var result = failMethod!.Invoke(null, new object[] { errors });
                    return (TResponse)result!;
                }
                if (responseType == typeof(Result))
                {
                    // Non-generic Result — call Result.Fail(errors)
                    var result = Result.Fail(errors);
                    return (TResponse)(object)result;
                }
                // If TResponse is not a Result type, we can't short-circuit gracefully
                // So we throw an exception as a fallback (shouldn't happen in practice)
                throw new ValidationException(validationResults.SelectMany(r => r.Errors));
            }

            // No validation errors — proceed to the handler
            return await next();
        }
    }
}
