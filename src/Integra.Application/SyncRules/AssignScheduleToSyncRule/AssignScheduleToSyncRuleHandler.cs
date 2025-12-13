using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Integra.Application.Exceptions;
using Integra.Application.Interfaces.Repositories;
using MediatR;

namespace Integra.Application.SyncRules.AssignScheduleToSyncRule;

public sealed class AssignScheduleToSyncRuleHandler
    : ICommandHandler<AssignScheduleToSyncRuleCommand, Unit>
{
    private readonly ISyncRuleRepository _repository;

    public AssignScheduleToSyncRuleHandler(ISyncRuleRepository repository)
    {
        _repository = repository;
    }

    public async Task<Unit> Handle(
        AssignScheduleToSyncRuleCommand cmd,
        CancellationToken ct)
    {
        var rule = await _repository.GetByIdAsync(cmd.SyncRuleId, ct)
            ?? throw new ApplicationNotFoundException("SyncRule not found");

        rule.AssignSchedule(cmd.SyncScheduleId);

        return Unit.Value;
    }
}
