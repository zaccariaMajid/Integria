using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Integra.Application.SyncRules.Queries.GetSyncRuleById;
using Integra.Domain.AggregateRoots;

namespace Integra.Application.Interfaces.Repositories.SyncRules;

public interface ISyncRuleRepository
{
    Task AddAsync(SyncRule rule);
    Task<SyncRule?> GetByIdAsync(Guid id, CancellationToken ct);
}
