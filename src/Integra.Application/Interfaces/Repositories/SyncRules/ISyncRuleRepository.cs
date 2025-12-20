using Integra.Domain.AggregateRoots;

namespace Integra.Application.Interfaces.Repositories.SyncRules;

public interface ISyncRuleRepository
{
    Task AddAsync(SyncRule rule);
    Task<SyncRule?> GetByIdAsync(Guid id, CancellationToken ct);
}
