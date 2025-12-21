using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Integra.Application.SyncJobs.Queries;

namespace Integra.Application.Interfaces.Repositories.SyncJobs;

public interface ISyncJobReadRepository
{
     Task<SyncJobDetailsDto?> GetByIdAsync(Guid jobId, CancellationToken ct);

    Task<IReadOnlyList<SyncJobListItemDto>> ListBySyncRuleAsync(
        Guid syncRuleId,
        CancellationToken ct);

    Task<IReadOnlyList<SyncJobListItemDto>> ListRecentAsync(
        Guid tenantId,
        int take,
        CancellationToken ct);
}
