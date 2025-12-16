using Integra.Application.Abstractions;

namespace Integra.Application.SyncRules.Queries.GetSyncRuleById;

public sealed record GetSyncRuleByIdQuery(Guid SyncRuleId)
    : IQuery<SyncRuleDetailsDto>;