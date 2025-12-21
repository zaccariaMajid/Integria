using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Integra.Application.Interfaces;
using Integra.Application.Interfaces.Repositories.SyncJobs;

namespace Integra.Application.SyncJobs.Queries.ListSyncJobsByRule;

public sealed class ListSyncJobsByRuleHandler
    : IQueryHandler<ListSyncJobsByRuleQuery, IReadOnlyList<SyncJobListItemDto>>
{
    private readonly ISyncJobReadRepository _repository;

    public ListSyncJobsByRuleHandler(ISyncJobReadRepository repository)
    {
        _repository = repository;
    }

    public async Task<IReadOnlyList<SyncJobListItemDto>> Handle(
        ListSyncJobsByRuleQuery query,
        CancellationToken ct)
    {
        return await _repository.ListBySyncRuleAsync(query.SyncRuleId, ct);
    }
}
