using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Integra.Application.Interfaces;
using Integra.Application.Interfaces.Repositories.SyncRules;

namespace Integra.Application.SyncRules.Queries.ListByTenant;

public sealed class ListSyncRulesByTenantHandler
    : IQueryHandler<ListSyncRulesByTenantQuery, IReadOnlyList<SyncRuleListItemDto>>
{
    private readonly ISyncRuleReadRepository _repository;

    public ListSyncRulesByTenantHandler(ISyncRuleReadRepository repository)
    {
        _repository = repository;
    }

    public async Task<IReadOnlyList<SyncRuleListItemDto>> Handle(
        ListSyncRulesByTenantQuery query,
        CancellationToken ct)
    {
        return await _repository.ListByTenantAsync(query.TenantId, ct);
    }
}
