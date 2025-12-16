using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Integra.Application.Abstractions;
using Integra.Application.Interfaces;
using MediatR;

namespace Integra.Application.Abstractions.Messaging;

public class UnitOfWorkBehavior<TRequest, TResponse>
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
    where TResponse : notnull
{
    private readonly IUnitOfWork _uow;

    public UnitOfWorkBehavior(IUnitOfWork uow)
    {
        _uow = uow;
    }

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken ct)
    {
        // Commands are handled by TransactionBehavior; queries are read-only.
        if (request is ICommand<TResponse> || request is IQuery<TResponse>)
            return await next();

        var response = await next();

        await _uow.SaveChangesAsync(ct);
        return response;
    }
}
