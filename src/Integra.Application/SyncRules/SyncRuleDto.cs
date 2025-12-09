using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Integra.Domain.AggregateRoots;
using Integra.Domain.Enums;

namespace Integra.Application.SyncRules;

public sealed record SyncRuleDto(
    Guid Id,
    Guid TenantId,
    Guid SourceIntegrationId,
    Guid TargetIntegrationId,
    SyncDirection Direction,
    SyncScope Scope,
    SyncConflictPolicy ConflictPolicy,
    Guid FieldMappingId,
    Guid? ScheduleId,
    bool IsEnabled
)
{
    public static SyncRuleDto FromDomain(SyncRule rule)
        => new(
            rule.Id,
            rule.TenantId,
            rule.SourceIntegrationId,
            rule.TargetIntegrationId,
            rule.Direction,
            rule.Scope,
            rule.ConflictPolicy,
            rule.FieldMappingId,
            rule.SyncScheduleId,
            rule.IsEnabled
        );
}

