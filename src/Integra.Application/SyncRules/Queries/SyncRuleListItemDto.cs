using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Integra.Domain.Enums;

namespace Integra.Application.SyncRules.Queries;

public sealed record SyncRuleListItemDto
(
    Guid Id,
    Guid SourceIntegrationId,
    Guid TargetIntegrationId,
    SyncDirection Direction,
    SyncScope Scope,
    bool IsEnabled,
    DateTime CreatedAt
);