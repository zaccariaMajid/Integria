using Integra.Application.SyncRules.Queries;
using Integra.Application.SyncRules.Queries.GetById;
using Integra.Application.SyncRules.Queries.ListByTenant;

namespace Integra.Application.Interfaces.Repositories.SyncRules;

public interface ISyncRuleReadRepository
{
    Task<SyncRuleDetailsDto?> GetByIdAsync(Guid id, CancellationToken ct);
    Task<IReadOnlyList<SyncRuleListItemDto>> ListByTenantAsync(
        Guid tenantId,
        CancellationToken ct);
    Task<IReadOnlyList<SyncRuleListItemDto>> ListByIntegrationAsync(
        Guid integrationId,
        CancellationToken ct);
}
