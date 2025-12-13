using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Integra.Application.Exceptions;
using Integra.Application.Interfaces.Repositories;
using MediatR;

namespace Integra.Application.SyncRules.Commands.EnableSyncRule;

public sealed class EnableSyncRuleHandler
    : ICommandHandler<EnableSyncRuleCommand, Unit>
{
    private readonly ISyncRuleRepository _repository;

    public EnableSyncRuleHandler(ISyncRuleRepository repository)
    {
        _repository = repository;
    }

    public async Task<Unit> Handle(
        EnableSyncRuleCommand cmd,
        CancellationToken ct)
    {
        var rule = await _repository.GetByIdAsync(cmd.SyncRuleId, ct)
            ?? throw new ApplicationNotFoundException("SyncRule not found");

        rule.Enable();

        return Unit.Value;
    }
}
