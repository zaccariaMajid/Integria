using Integra.Application.Exceptions;
using Integra.Application.Interfaces.Repositories;
using Integra.Application.Interfaces.Repositories.SyncRules;
using MediatR;

namespace Integra.Application.SyncRules.Commands.DisableSyncRule;

public sealed class DisableSyncRuleHandler
    : IRequestHandler<DisableSyncRuleCommand, Unit>
{
    private readonly ISyncRuleRepository _repository;

    public DisableSyncRuleHandler(ISyncRuleRepository repository)
    {
        _repository = repository;
    }

    public async Task<Unit> Handle(
        DisableSyncRuleCommand cmd,
        CancellationToken ct)
    {
        var rule = await _repository.GetByIdAsync(cmd.SyncRuleId, ct)
            ?? throw new ApplicationNotFoundException("SyncRule not found");

        rule.Disable();

        return Unit.Value;
    }
}
