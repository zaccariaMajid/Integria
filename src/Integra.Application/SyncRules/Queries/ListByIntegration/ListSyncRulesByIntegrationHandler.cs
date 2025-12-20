using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Integra.Application.Interfaces;
using Integra.Application.Interfaces.Repositories.SyncRules;

namespace Integra.Application.SyncRules.Queries.ListByIntegration;

public sealed class ListSyncRulesByIntegrationHandler
    : IQueryHandler<ListSyncRulesByIntegrationQuery, IReadOnlyList<SyncRuleListItemDto>>
{
    private readonly ISyncRuleReadRepository _repository;

    public ListSyncRulesByIntegrationHandler(ISyncRuleReadRepository repository)
    {
        _repository = repository;
    }

    public async Task<IReadOnlyList<SyncRuleListItemDto>> Handle(
        ListSyncRulesByIntegrationQuery query,
        CancellationToken ct)
    {
        return await _repository.ListByIntegrationAsync(query.IntegrationId, ct);
    }
}
