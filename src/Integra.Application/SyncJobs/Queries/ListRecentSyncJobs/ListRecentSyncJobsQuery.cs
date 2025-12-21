using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Integra.Application.Interfaces;

namespace Integra.Application.SyncJobs.Queries.ListRecentSyncJobs;

public sealed record ListRecentSyncJobsQuery(
    Guid TenantId,
    int Take = 20
) : IQuery<IReadOnlyList<SyncJobListItemDto>>;

