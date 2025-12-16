using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Integra.Application.Exceptions;
using Integra.Application.Interfaces.Repositories;
using Integra.Application.Interfaces.Repositories.SyncRules;
using MediatR;

namespace Integra.Application.SyncRules.Commands.RemoveScheduleFromSyncRule;

public sealed class RemoveScheduleFromSyncRuleHandler
    : ICommandHandler<RemoveScheduleFromSyncRuleCommand, Unit>
{
    private readonly ISyncRuleRepository _repository;

    public RemoveScheduleFromSyncRuleHandler(ISyncRuleRepository repository)
    {
        _repository = repository;
    }

    public async Task<Unit> Handle(
        RemoveScheduleFromSyncRuleCommand cmd,
        CancellationToken ct)
    {
        var rule = await _repository.GetByIdAsync(cmd.SyncRuleId, ct)
            ?? throw new ApplicationNotFoundException("SyncRule not found");

        rule.RemoveSchedule();

        return Unit.Value;
    }
}
