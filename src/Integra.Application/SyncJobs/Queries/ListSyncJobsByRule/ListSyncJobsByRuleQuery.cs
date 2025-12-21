using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Integra.Application.Interfaces;

namespace Integra.Application.SyncJobs.Queries.ListSyncJobsByRule;

public sealed record ListSyncJobsByRuleQuery(Guid SyncRuleId)
    : IQuery<IReadOnlyList<SyncJobListItemDto>>;

