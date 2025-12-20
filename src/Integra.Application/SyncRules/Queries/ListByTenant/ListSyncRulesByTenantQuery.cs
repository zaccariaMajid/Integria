using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Integra.Application.Interfaces;

namespace Integra.Application.SyncRules.Queries.ListByTenant;

public sealed record ListSyncRulesByTenantQuery(Guid TenantId)
    : IQuery<IReadOnlyList<SyncRuleListItemDto>>;