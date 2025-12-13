using Integra.Application.Interfaces;
using Integra.Application.SyncRules;
using Integra.Domain.Enums;
using MediatR;

namespace Integra.Application.SyncRules.UpdateSyncRule;

public sealed record UpdateSyncRuleCommand(
    Guid SyncRuleId,
    SyncScope Scope,
    SyncFilterDto? Filter,
    SyncConflictPolicy ConflictPolicy,
    Guid FieldMappingId
) : IRequest<Unit>, ICommand<Unit>;
