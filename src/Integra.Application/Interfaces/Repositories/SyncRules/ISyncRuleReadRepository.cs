using Integra.Application.SyncRules.Queries.GetSyncRuleById;

namespace Integra.Application.Interfaces.Repositories.SyncRules;

public interface ISyncRuleReadRepository
{
    Task<SyncRuleDetailsDto?> GetByIdAsync(Guid id, CancellationToken ct);
    Task<IReadOnlyList<SyncRuleDetailsDto>> ListByTenantAsync(
        Guid tenantId,
        CancellationToken ct);
}
