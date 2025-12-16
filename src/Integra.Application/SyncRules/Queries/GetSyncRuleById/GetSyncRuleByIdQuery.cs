using Integra.Application.Interfaces;

namespace Integra.Application.SyncRules.Queries.GetSyncRuleById;

public sealed record GetSyncRuleByIdQuery(Guid SyncRuleId)
    : IQuery<SyncRuleDetailsDto>;
