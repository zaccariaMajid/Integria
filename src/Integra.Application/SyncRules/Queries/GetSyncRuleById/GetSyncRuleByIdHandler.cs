using Integra.Application.Exceptions;
using Integra.Application.Interfaces;
using Integra.Application.Interfaces.Repositories.SyncRules;

namespace Integra.Application.SyncRules.Queries.GetSyncRuleById;

public sealed class GetSyncRuleByIdHandler
    : IQueryHandler<GetSyncRuleByIdQuery, SyncRuleDetailsDto>
{
    private readonly ISyncRuleReadRepository _repository;

    public GetSyncRuleByIdHandler(ISyncRuleReadRepository repository)
    {
        _repository = repository;
    }

    public async Task<SyncRuleDetailsDto> Handle(
        GetSyncRuleByIdQuery query,
        CancellationToken ct)
    {
        var dto = await _repository.GetByIdAsync(query.SyncRuleId, ct);

        return dto ?? throw new ApplicationNotFoundException($"SyncRule {query.SyncRuleId} not found.");
    }
}
