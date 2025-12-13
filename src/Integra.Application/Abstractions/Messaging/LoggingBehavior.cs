using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Integra.Application.Abstractions.Messaging;

public class LoggingBehavior<TRequest, TResponse>
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    private readonly ILogger<LoggingBehavior<TRequest, TResponse>> _logger;

    public LoggingBehavior(ILogger<LoggingBehavior<TRequest, TResponse>> logger)
    {
        _logger = logger;
    }

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken ct)
    {
        var requestName = typeof(TRequest).Name;
        var correlationId = GetCorrelationId(request);
        var tenantId = GetTenantId(request);

        _logger.LogInformation(
            "[START] {RequestName} | Tenant: {TenantId} | Correlation: {CorrelationId} | Payload: {@Request}",
            requestName,
            tenantId,
            correlationId,
            request
        );

        var stopwatch = Stopwatch.StartNew();

        try
        {
            var response = await next();

            stopwatch.Stop();

            _logger.LogInformation(
                "[END] {RequestName} | Time: {Elapsed} ms | Tenant: {TenantId} | Correlation: {CorrelationId}",
                requestName,
                stopwatch.ElapsedMilliseconds,
                tenantId,
                correlationId
            );

            return response;
        }
        catch (Exception ex)
        {
            stopwatch.Stop();

            _logger.LogError(
                ex,
                "[ERROR] {RequestName} failed after {Elapsed} ms | Tenant: {TenantId} | Correlation: {CorrelationId}",
                requestName,
                stopwatch.ElapsedMilliseconds,
                tenantId,
                correlationId
            );

            throw;
        }
    }

    // OPTIONAL: Try to extract CorrelationId if command has it
    private static string? GetCorrelationId(TRequest request)
    {
        var prop = typeof(TRequest).GetProperty("CorrelationId");
        return prop?.GetValue(request)?.ToString();
    }

    private static Guid? GetTenantId(TRequest request)
    {
        var prop = typeof(TRequest).GetProperty("TenantId");
        if (prop?.GetValue(request) is Guid id)
            return id;
        return null;
    }
}
