using Integra.Application.Exceptions;
using Integra.Application.Interfaces.Repositories;
using MediatR;

namespace Integra.Application.SyncRules.UpdateSyncRule;

public sealed class UpdateSyncRuleHandler
    : IRequestHandler<UpdateSyncRuleCommand, Unit>
{
    private readonly ISyncRuleRepository _repository;

    public UpdateSyncRuleHandler(ISyncRuleRepository repository)
    {
        _repository = repository;
    }

    public async Task<Unit> Handle(
        UpdateSyncRuleCommand cmd,
        CancellationToken ct)
    {
        var rule = await _repository.GetByIdAsync(cmd.SyncRuleId, ct)
            ?? throw new ApplicationNotFoundException($"SyncRule {cmd.SyncRuleId} not found.");

        rule.UpdateConfiguration(
            scope: cmd.Scope,
            filter: cmd.Filter?.ToValueObject(),
            conflictPolicy: cmd.ConflictPolicy,
            fieldMappingId: cmd.FieldMappingId
        );

        return Unit.Value;
    }
}
