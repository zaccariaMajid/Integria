using Integra.Application.Interfaces;
using MediatR;

namespace Integra.Application.SyncRules.Commands.DisableSyncRule;

public sealed record DisableSyncRuleCommand(Guid SyncRuleId) : ICommand<Unit>;
