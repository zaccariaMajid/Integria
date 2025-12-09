using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Integra.Application.Interfaces;
using Integra.Domain.Enums;

namespace Integra.Application.SyncRules.CreateSyncRule;

public sealed record CreateSyncRuleCommand(
    Guid TenantId,
    Guid SourceIntegrationId,
    Guid TargetIntegrationId,
    SyncDirection Direction,
    SyncScope Scope,
    SyncFilterDto? Filter,
    SyncConflictPolicy ConflictPolicy,
    Guid FieldMappingId,
    Guid? SyncScheduleId
) : ICommand<SyncRuleDto>;
