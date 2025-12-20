using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Integra.Application.Interfaces;
using Integra.Application.SyncRules.Queries.ListByTenant;

namespace Integra.Application.SyncRules.Queries.ListByIntegration;

public sealed record ListSyncRulesByIntegrationQuery(Guid IntegrationId)
    : IQuery<IReadOnlyList<SyncRuleListItemDto>>;
