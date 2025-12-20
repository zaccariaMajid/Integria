using Integra.Application.Interfaces;

namespace Integra.Application.SyncRules.Queries.GetById;

public sealed record GetSyncRuleByIdQuery(Guid SyncRuleId)
    : IQuery<SyncRuleDetailsDto>;
