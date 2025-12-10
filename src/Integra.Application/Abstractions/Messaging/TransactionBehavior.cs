using System;
using System.Transactions;
using Integra.Application.Abstractions;
using Integra.Application.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Integra.Application.Abstractions.Messaging;

/// <summary>
/// Wraps command handlers in a transaction scope and commits the unit of work only on success.
/// Queries are passed through untouched to avoid unnecessary transaction overhead.
/// </summary>
public sealed class TransactionBehavior<TRequest, TResponse>
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
    where TResponse : notnull
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<TransactionBehavior<TRequest, TResponse>> _logger;

    public TransactionBehavior(
        IUnitOfWork unitOfWork,
        ILogger<TransactionBehavior<TRequest, TResponse>> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken ct)
    {
        // Only commands mutate state; let queries continue without a transaction.
        if (request is not ICommand<TResponse>)
            return await next();

        // Avoid nesting scopes when upstream already started one.
        if (Transaction.Current is not null)
            return await ExecuteAndCommitAsync(next, ct);

        using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

        var response = await ExecuteAndCommitAsync(next, ct);

        scope.Complete();

        return response;
    }

    private async Task<TResponse> ExecuteAndCommitAsync(
        RequestHandlerDelegate<TResponse> next,
        CancellationToken ct)
    {
        try
        {
            var response = await next();
            await _unitOfWork.SaveChangesAsync(ct);
            return response;
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Transaction failed for {RequestName}",
                typeof(TRequest).Name);

            throw;
        }
    }
}