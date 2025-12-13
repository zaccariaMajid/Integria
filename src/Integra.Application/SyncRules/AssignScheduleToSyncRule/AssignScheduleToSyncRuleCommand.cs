using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Integra.Application.Interfaces;
using MediatR;

namespace Integra.Application.SyncRules.AssignScheduleToSyncRule;

public sealed record AssignScheduleToSyncRuleCommand(
    Guid SyncRuleId,
    Guid SyncScheduleId
) : ICommand<Unit>;
