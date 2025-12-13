using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Integra.Application.Interfaces;
using MediatR;

namespace Integra.Application.SyncRules.Commands.RemoveScheduleFromSyncRule;

public sealed record RemoveScheduleFromSyncRuleCommand(Guid SyncRuleId) : ICommand<Unit>;