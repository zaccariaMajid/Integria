using Integra.Domain.Enums;

namespace Integra.Application.SyncRules.Queries.GetById;

public sealed record SyncRuleDetailsDto
(
    Guid Id,
    Guid TenantId,
    Guid SourceIntegrationId,
    Guid TargetIntegrationId,
    SyncDirection Direction,
    SyncScope Scope,
    SyncConflictPolicy ConflictPolicy,
    Guid FieldMappingId,
    Guid? SyncScheduleId,
    bool IsEnabled,
    DateTime CreatedAt,
    DateTime? UpdatedAt
);
