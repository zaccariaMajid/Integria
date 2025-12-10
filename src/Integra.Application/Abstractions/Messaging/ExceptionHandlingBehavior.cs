
using Integra.Application.Exceptions;
using Integra.Domain.Exceptions;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Integra.Application.Abstractions.Messaging;

public sealed class ExceptionHandlingBehavior<TRequest, TResponse>
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
    where TResponse : notnull
{
    private readonly ILogger<ExceptionHandlingBehavior<TRequest, TResponse>> _logger;

    public ExceptionHandlingBehavior(
        ILogger<ExceptionHandlingBehavior<TRequest, TResponse>> logger)
    {
        _logger = logger;
    }

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken ct)
    {
        try
        {
            return await next();
        }
        catch (ApplicationValidationException ex)
        {
            _logger.LogWarning(ex,
                "Validation failed for request {RequestName}: {Errors}",
                typeof(TRequest).Name,
                string.Join(", ", ex.Errors.Select(e => e.ErrorMessage)));

            throw new ApplicationValidationException(ex.Errors);
        }
        catch (DomainException ex)
        {
            _logger.LogWarning(ex,
                "Domain exception in request {RequestName}: {Message}",
                typeof(TRequest).Name,
                ex.Message);

            // Qui convertiamo DomainException in una exception coerente con l'API
            throw new ApplicationDomainException(ex.Message);
        }
        catch (ApplicationNotFoundException ex)
        {
            _logger.LogWarning(ex,
                "Resource not found in request {RequestName}: {Message}",
                typeof(TRequest).Name,
                ex.Message);

            throw new ApplicationNotFoundException(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,
                "Unhandled exception for request {RequestName}",
                typeof(TRequest).Name);

            throw new ApplicationUnhandledException("An unexpected error occurred.", ex);
        }
    }
}
