using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Integra.Application.Interfaces;
using Integra.Application.Interfaces.Repositories.SyncJobs;

namespace Integra.Application.SyncJobs.Queries.ListRecentSyncJobs;

public sealed class ListRecentSyncJobsHandler
    : IQueryHandler<ListRecentSyncJobsQuery, IReadOnlyList<SyncJobListItemDto>>
{
    private readonly ISyncJobReadRepository _repository;

    public ListRecentSyncJobsHandler(ISyncJobReadRepository repository)
    {
        _repository = repository;
    }

    public async Task<IReadOnlyList<SyncJobListItemDto>> Handle(
        ListRecentSyncJobsQuery query,
        CancellationToken ct)
    {
        return await _repository.ListRecentAsync(
            query.TenantId,
            query.Take,
            ct);
    }
}
